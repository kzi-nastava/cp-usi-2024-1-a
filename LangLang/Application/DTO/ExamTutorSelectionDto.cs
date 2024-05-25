using LangLang.Domain.Model;
using System;
using System.Collections.Generic;

namespace LangLang.Application.DTO;

public class ExamTutorSelectionDto
{
    public Language Language { get; }
    public LanguageLevel LanguageLevel { get; }
    public DateOnly? Date { get; }
    public TimeOnly? Time { get; }
    public Exam? ExceptionExam { get; }

    public ExamTutorSelectionDto(Language language, LanguageLevel languageLevel, DateOnly date, TimeOnly time, Exam? exceptionExam = null)
    {
        Language = language;
        LanguageLevel = languageLevel;
        Date = date;
        Time = time;
        ExceptionExam = exceptionExam;
    }

}
