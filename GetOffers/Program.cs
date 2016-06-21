#region Copyright, Author Details and Related Context   
// Copyright (C) SquidEyes, LLC. - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and Confidential
// Written by Louis S. Berman <louis@squideyes.com>, 6/20/2016
#endregion  

using System;
using System.Threading;

namespace GetOffers
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to cancel...");
            Console.WriteLine();

            var closeEvent = new EventWaitHandle(
                false, EventResetMode.AutoReset);

            var client = new OfferClient(
                Properties.Settings.Default.UserName,
                Properties.Settings.Default.Password,
                Properties.Settings.Default.Connection.ToEnum<Connection>());

            client.OnDisconnected += (s, e) =>
            {
                Console.WriteLine();
                Console.Write("Press any key to terminate...");

                Console.ReadKey(true);

                closeEvent.Set();
            };

            client.OnStatus += (s, e) => Log("STATUS", e.Item.ToString());

            client.OnOffer += (s, e) => Log("OFFER", e.Item.ToString());

            client.OnError += (s, e) => Log("ERROR", e.Item.Message);

            client.OnCancelled += (s, e) => Log(
                "CANCEL", "The connection was manually cancelled!");

            client.Start();

            Console.ReadKey(true);

            client.Cancel();

            closeEvent.WaitOne();
        }

        private static void Log(string kind, string message) =>
            Console.WriteLine($"{kind,-8} {message}");
    }
}
