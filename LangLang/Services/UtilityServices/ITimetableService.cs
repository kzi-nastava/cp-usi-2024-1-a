using System;
using System.Collections.Generic;
using Consts;
using LangLang.Model;

namespace LangLang.Services.UtilityServices;

public interface ITimetableService
{
    public List<TimeOnly> GetAvailableExamTimes(DateOnly date, Tutor tutor);

    public Dictionary<WorkDay, List<TimeOnly>> GetAvailableLessonTimes(DateOnly start, int numOfWeeks, Tutor tutor);

    public List<int> GetAvailableClassrooms(DateOnly date, TimeOnly time, TimeSpan duration, Tutor tutor);
}