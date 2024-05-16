using LangLang.Domain.Model;
using System;

namespace LangLang.WPF.ViewModels.Student;

public class ExamViewModel
{
    public string Id { get; set; }
    public Language Language { get; set; }
    public LanguageLevel LanguageLevel { get; set; }
    public DateTime Time { get; set; }
    public int ClassroomNumber { get; set; }
    public int MaxStudents { get; set; }

    public ExamViewModel(Exam exam)
    {
        Id = exam.Id;
        Language = exam.Language;
        LanguageLevel = exam.LanguageLevel;
        Time = exam.Time;
        ClassroomNumber = exam.ClassroomNumber;
        MaxStudents = exam.MaxStudents;
    }

}
