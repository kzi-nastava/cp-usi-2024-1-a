using System;
using LangLang.Domain;
using LangLang.Domain.Model;

namespace LangLang.Application.DTO;

public class ExamDto
{
    public string? Id { get; }
    public Language? Language { get; }
    public LanguageLevel? LanguageLevel { get; }
    public DateOnly? Date { get; }
    public TimeOnly? Time { get; }
    public int ClassroomNumber { get; }
    public int MaxStudents { get; }
    public Tutor? Tutor { get; }

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
        if (Language == null || LanguageLevel == null || Date == null || Time == null ||
            Tutor == null || MaxStudents <= 0 ||
            ClassroomNumber <= 0 || ClassroomNumber > Constants.ClassroomsNumber)
            return false;

        if (Date.Value.ToDateTime(Time.Value).Subtract(Constants.LockedExamTime) < DateTime.Now)
            return false;

        return true;
    }
}