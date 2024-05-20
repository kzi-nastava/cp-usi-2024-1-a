using System;
using System.Collections.Generic;
using LangLang.Domain.Model;

namespace LangLang.Application.Utility.Timetable;

public interface ITimetableService
{
    public List<TimeOnly> GetAvailableExamTimes(DateOnly date, Tutor tutor);

    public Dictionary<WorkDay, List<TimeOnly>> GetAvailableLessonTimes(DateOnly start, int numOfWeeks, Tutor tutor);

    public List<int> GetAvailableClassrooms(DateOnly date, TimeOnly time, TimeSpan duration, Tutor tutor);

    public List<TimeOnly> GetAllExamTimes();

    public List<TimeOnly> GetAllLessonTimes();
}