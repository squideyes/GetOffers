#region Copyright, Author Details and Related Context   
// Copyright (C) SquidEyes, LLC. - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and Confidential
// Written by Louis S. Berman <louis@squideyes.com>, 6/20/2016
#endregion  

using System;

namespace GetOffers
{
    public static class DateTimeExtenders
    {
        private const string EST_TZNAME = "Eastern Standard Time";
        private const string UTC_TZNAME = "UTC";

        private static readonly TimeZoneInfo estTzi;
        private static readonly TimeZoneInfo utcTzi;

        static DateTimeExtenders()
        {
            estTzi = TimeZoneInfo.FindSystemTimeZoneById(EST_TZNAME);
            utcTzi = TimeZoneInfo.FindSystemTimeZoneById(UTC_TZNAME);
        }

        public static DateTime ToEstFromUtc(this DateTime dateTime)
        {
            if (dateTime.Kind != DateTimeKind.Utc)
                throw new ArgumentOutOfRangeException("dateTime.Kind");

            return TimeZoneInfo.ConvertTime(dateTime, estTzi);
        }

        public static bool IsWeekday(this DateTime value)
        {
            if (value.Kind != DateTimeKind.Unspecified)
                throw new ArgumentOutOfRangeException(nameof(value));

            switch (value.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                case DayOfWeek.Sunday:
                    return false;
                default:
                    return true;
            }
        }

        public static bool IsTickOn(this DateTime value)
        {
            if (!value.IsWeekday())
                return false;

            var timeOfDay = value.TimeOfDay;

            return (timeOfDay >= WellKnown.SessionStart)
                && (timeOfDay < WellKnown.SessionEnd);
        }

        public static DateTime TruncateToSecond(this DateTime value) =>
            value.AddMilliseconds(-value.Millisecond);

        public static DateTime TruncateToMinute(this DateTime value) =>
            new DateTime(value.Ticks - (value.Ticks % TimeSpan.TicksPerMinute));

        public static string ToTickOnString(this DateTime value) =>
            value.ToString("MM/dd/yyyy HH:mm:ss.fff");
    }
}
