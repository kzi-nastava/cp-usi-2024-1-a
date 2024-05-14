namespace LangLang.WPF.ViewModels.Tutor.Exam;

public class StudentAddedStatusViewModel
{
    public Domain.Model.Student Student { get; }
    public string Name { get; }
    public string Surname { get; }
    public bool Added { get; }

    public StudentAddedStatusViewModel(Domain.Model.Student student, bool added)
    {
        Student = student;
        Added = added;
        Name = student.Name;
        Surname = student.Surname;
    }
}