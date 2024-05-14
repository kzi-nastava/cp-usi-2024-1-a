using System.Collections.Generic;
using LangLang.Domain.Model;

namespace LangLang.Domain.RepositoryInterfaces;

public interface IStudentDAO
{
    public Dictionary<string, Student> GetAllStudents();
    public Student? GetStudent(string id);
    public Student AddStudent(Student student);
    public Student UpdateStudent(Student student);
    public void DeleteStudent(string id);
    
}