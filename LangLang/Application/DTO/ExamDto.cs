using System;
using LangLang.Domain;
using LangLang.Domain.Model;

namespace LangLang.Application.DTO;

public class ExamDto
{
    [SkipInForm] public string? Id { get; set; }
    public Language? Language { get; set; }
    public LanguageLevel? LanguageLevel { get; set; }
    public DateOnly? Date { get; set; }
    public TimeOnly? Time { get; set; }
    public int ClassroomNumber { get; set; }
    public int MaxStudents { get; set; }
    
    [SkipInForm] public Tutor? Tutor { get; set; }

    public ExamDto()
    {
    }

    public ExamDto(string? id, Language? language, LanguageLevel? languageLevel, DateOnly? date, TimeOnly? time, int classroomNumber, int maxStudents, Tutor? tutor = null)
    {
        Id = id;
        Language = language;
        LanguageLevel = languageLevel;
        Date = date;
        Time = time;
        ClassroomNumber = classroomNumber;
        MaxStudents = maxStudents;
        Tutor = tutor;
    }

    public bool IsValid()
    {
        if (Language == null || LanguageLevel == null || Date == null || Time == null || MaxStudents <= 0 ||
            ClassroomNumber <= 0 || ClassroomNumber > Constants.ClassroomsNumber)
            return false;

        if (Date.Value.ToDateTime(Time.Value).Subtract(Constants.LockedExamTime) < DateTime.Now)
            return false;

        return true;
    }
}