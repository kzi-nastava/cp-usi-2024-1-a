using LangLang.Domain.Model;
using System;

namespace LangLang.WPF.ViewModels.Student;

public class CourseViewModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Language Language { get; set; }
    public LanguageLevel Level { get; set; }
    public int Duration { get; set; }
    public DateTime Start { get; set; }
    public bool Online { get; set; }
    public string TutorFullName {  get; set; }
    public CourseViewModel(Course course, string tutorFullName)
    {
        Id = course.Id;
        Name = course.Name;
        Language = course.Language;
        Level = course.Level;
        Duration = course.Duration;
        Start = course.Start;
        Online = course.Online;
        TutorFullName = tutorFullName;
    }
}
