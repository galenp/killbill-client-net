using System;

namespace KillBill.Client.Net.Infrastructure
{
    public static class DateTimeHelperExtensions
    {
        public static string ToDateTimeISO(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH':'mm':'ss");
        }

        public static string ToDateString(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }
    }
}