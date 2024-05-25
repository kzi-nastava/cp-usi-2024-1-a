using System;

namespace LangLang.WPF.ViewModels.Tutor.Exam;

public class ExamViewModel
{
    public string Id { get; }
    public string LanguageName { get; }
    public string LanguageLevel { get; }
    public DateOnly Date { get; }
    public TimeOnly Time { get; }
    public int MaxStudents { get; }
    public int NumStudents { get; }
    public string State { get; }
    public bool HasTutor { get; set; }

    private ExamViewModel(string id, string languageName, string languageLevel, DateOnly date, TimeOnly time, int maxStudents,
        int numStudents, string state, bool hasTutor)
    {
        Id = id;
        LanguageName = languageName;
        LanguageLevel = languageLevel;
        Date = date;
        Time = time;
        MaxStudents = maxStudents;
        NumStudents = numStudents;
        State = state;
        HasTutor = hasTutor;
    }

    public ExamViewModel(Domain.Model.Exam exam) : this(
        exam.Id,
        exam.Language.Name,
        exam.LanguageLevel.ToString(),
        exam.Date,
        exam.TimeOfDay,
        exam.MaxStudents,
        exam.NumStudents,
        exam.ExamState.ToString(),
        exam.TutorId != null
    )
    {
    }
}