using System;
using LangLang.Domain.Model;

namespace LangLang.Repositories.Json.Util;

public class DayConverter
{
    public static WorkDay ToWorkDay(DayOfWeek day)
    {
        if (day is DayOfWeek.Saturday or DayOfWeek.Sunday)
        {
            throw new ArgumentException("Weekend not allowed.");
        }
        return (WorkDay)((int)day - 1);
    }

    public static DayOfWeek ToDayOfWeek(WorkDay day)
    {
        return (DayOfWeek)((int)day + 1);
    }
}