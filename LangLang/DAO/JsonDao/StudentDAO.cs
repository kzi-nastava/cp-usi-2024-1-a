using System.Collections.Generic;
using Consts;
using LangLang.Model;
using LangLang.Util;

namespace LangLang.DAO.JsonDao;

public class StudentDAO : IStudentDAO
{
    private Dictionary<string, Student>? _students;

    private Dictionary<string, Student> Students
    {
        get {
            _students ??= JsonUtil.ReadFromFile<Student>(Constants.StudentFilePath);
            return _students!;
        }
        set => _students = value;
    }

    public Dictionary<string, Student> GetAllStudents()
    {
        return Students;
    }

    public Student? GetStudent(string email)
    {
        return Students.GetValueOrDefault(email);
    }

    public Student AddStudent(Student student)
    {
        _students![student.Id] = student;
        SaveStudents();
        return student;
    }

    public void DeleteStudent(string email)
    {
        _students!.Remove(email);
        SaveStudents();
    }

    private void SaveStudents()
    {
        JsonUtil.WriteToFile(Students, Constants.StudentFilePath);
    }

}