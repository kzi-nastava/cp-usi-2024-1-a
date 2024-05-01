using System;

namespace LangLang.Util
{
    internal class DateTimeMilisecondsConverter
    {
        public static Int64 ToMiliseconds(DateTime date)
        {
            TimeSpan timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1);
            return (Int64)timeSpan.TotalMilliseconds;
        }

        public static DateTime ToDateTime(Int64 miliseconds)
        {
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(miliseconds);
            return new DateTime(1970, 1, 1) + timeSpan;
        }
    }
}
