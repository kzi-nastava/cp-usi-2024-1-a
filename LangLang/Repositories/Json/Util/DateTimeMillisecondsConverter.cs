using System;

namespace LangLang.Repositories.Json.Util
{
    internal static class DateTimeMillisecondsConverter
    {
        public static Int64 ToMilliseconds(DateTime dateTime)
        {
            TimeSpan timeSpan = dateTime - new DateTime(1970, 1, 1);
            return (Int64)timeSpan.TotalMilliseconds;
        }

        public static DateTime ToDateTime(Int64 milliseconds)
        {
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(milliseconds);
            return new DateTime(1970, 1, 1) + timeSpan;
        }
    }
}
