using System;
using System.Collections.Generic;
using System.Linq;
using LangLang.Application.DTO;
using LangLang.Application.UseCases.User;
using LangLang.Application.Utility.Timetable;
using LangLang.Domain.Model;

namespace LangLang.Application.UseCases.TutorSelection;

public class AutoCourseTutorSelector : IAutoCourseTutorSelector
{
    private readonly ITutorService _tutorService;
    private readonly ITimetableService _timetableService;

    public AutoCourseTutorSelector(ITutorService tutorService, ITimetableService timetableService)
    {
        _tutorService = tutorService;
        _timetableService = timetableService;
    }

    public Tutor? Select(CourseTutorSelectionDto dto)
    {
        var tutors = FilterEligibleTutors(_tutorService.GetAllTutors(), dto);
        return tutors.MaxBy(tutor => tutor.GetScore(dto.Language, dto.LanguageLevel));
    }

    private List<Tutor> FilterEligibleTutors(List<Tutor> tutors, CourseTutorSelectionDto dto)
    {
        return tutors.Where(tutor =>
            tutor.KnowsLanguage(dto.Language, dto.LanguageLevel) &&
            dto.Start != null &&
            IsAvailableForSchedule(tutor, dto.Schedule, dto.ScheduleDays, dto.Start.Value, dto.Duration)
        ).ToList();
    }

    private bool IsAvailableForSchedule(Tutor tutor, Dictionary<WorkDay, Tuple<TimeOnly, int>> schedule,
        List<WorkDay> scheduleDays, DateTime start, int duration)
    {
        var availableTimes =
            _timetableService.GetAvailableLessonTimes(DateOnly.FromDateTime(start), duration, tutor);
        foreach (var day in scheduleDays)
        {
            if (!availableTimes[day].Contains(schedule[day].Item1))
                return false;
        }
        return true;
    }
}