using System;
using System.Globalization;

namespace Robot.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfDay(this DateTime date)
        {
            if (date == null)
                throw new ArgumentNullException("date");

            return new DateTime(date.Year, date.Month, date.Day);
        }

        public static DateTime StartOfMonth(this DateTime date)
        {
            if (date == null)
                throw new ArgumentNullException("date");

            return new DateTime(date.Year, date.Month, 1);
        }

        public static DateTime EndOfMonth(this DateTime date)
        {
            if(date == null)
                throw new ArgumentNullException("date");

            return new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1);
        }

        public static int GetMonthNumberFromMonthString(this string monthName)
        {
            return DateTime.ParseExact(monthName, "MMMM", CultureInfo.CurrentCulture).Month;
        }

        public static bool IsNullDate(this DateTime date)
        {
            return date == new DateTime(1900, 1, 1);
        }
    }
}
