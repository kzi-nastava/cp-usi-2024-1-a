using LangLang.Application.DTO;
using LangLang.Application.UseCases.User;
using LangLang.Application.Utility.Timetable;
using LangLang.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LangLang.Application.UseCases.TutorSelection;

public class AutoExamTutorSelector : IAutoExamTutorSelector
{
    private readonly ITutorService _tutorService;
    private readonly ITimetableService _timetableService;

    public AutoExamTutorSelector(ITutorService tutorService, ITimetableService timetableService)
    {
        _tutorService = tutorService;
        _timetableService = timetableService;
    }

    public Tutor? Select(ExamTutorSelectionDto dto)
    {
        var tutors = FilterEligibleTutors(_tutorService.GetAllTutors(), dto);
        return tutors.MaxBy(tutor => tutor.GetScore(dto.Language, dto.LanguageLevel));
    }

    private List<Tutor> FilterEligibleTutors(List<Tutor> tutors, ExamTutorSelectionDto dto)
    {
        return tutors.Where(tutor =>
            tutor.KnowsLanguage(dto.Language, dto.LanguageLevel) &&
            dto.Time != null && dto.Date != null &&
            IsAvaliableForExam(tutor, dto.Date.Value, dto.Time.Value, dto.ExceptionExam)
            ).ToList();
    }

    private bool IsAvaliableForExam(Tutor tutor, DateOnly date,TimeOnly time,  Domain.Model.Exam? exam)
    {
        List<TimeOnly> avaliableTimes = _timetableService.GetAvailableExamTimes(date, tutor, null, exam);
        return avaliableTimes.Contains(time);
    }
}
