using System;
using System.Collections.Generic;
using LangLang.Domain.Model;

namespace LangLang.Application.DTO;

public class CourseTutorSelectionDto
{
    public Language Language { get; }
    public LanguageLevel LanguageLevel { get; }
    public int Duration { get; }
    public Dictionary<WorkDay, Tuple<TimeOnly, int>> Schedule { get; }
    public List<WorkDay> ScheduleDays { get; }
    public DateTime? Start { get; }

    public CourseTutorSelectionDto(Language language, LanguageLevel languageLevel, int duration, Dictionary<WorkDay, Tuple<TimeOnly, int>> schedule, List<WorkDay> scheduleDays, DateTime? start)
    {
        Language = language;
        LanguageLevel = languageLevel;
        Duration = duration;
        Schedule = schedule;
        ScheduleDays = scheduleDays;
        Start = start;
    }
}