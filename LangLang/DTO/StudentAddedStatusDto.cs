using LangLang.Model;
using System.ComponentModel;

namespace LangLang.DTO;

public class StudentAddedStatusDto
{
    public Student Student { get; }
    public string Name { get; }
    public string Surname { get; }
    public bool Added { get; }

    public StudentAddedStatusDto(Student student, bool added)
    {
        Student = student;
        Added = added;
        Name = student.Name;
        Surname = student.Surname;
    }
}