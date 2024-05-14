using System.Collections.Generic;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;
using LangLang.Repositories.Json.Util;

namespace LangLang.Repositories.Json;

public class StudentDAO : IStudentDAO
{
    private ILastIdDAO _lastIdDao;

    public StudentDAO(ILastIdDAO lastIdDao)
    {
        _lastIdDao = lastIdDao;
    }

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

    public Student? GetStudent(string id)
    {
        return Students.GetValueOrDefault(id);
    }

    public Student AddStudent(Student student)
    {
        _lastIdDao.IncrementStudentId();
        student.Id = _lastIdDao.GetStudentId();
        Students[student.Id] = student;
        SaveStudents();
        return student;
    }

    public Student UpdateStudent(Student student)
    {
        Students[student.Id] = student;
        SaveStudents();
        return student;
    }

    public void DeleteStudent(string id)
    {
        Students.Remove(id);
        SaveStudents();
    }

    private void SaveStudents()
    {
        JsonUtil.WriteToFile(Students, Constants.StudentFilePath);
    }

}