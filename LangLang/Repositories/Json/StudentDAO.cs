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
   
    public Dictionary<string, Student> GetAllStudents()
    {
        return JsonUtil.ReadFromFile<Student>(Constants.StudentFilePath);
    }

    public Student? GetStudent(string id)
    {
        Dictionary<string, Student> students = GetAllStudents();
        return students.GetValueOrDefault(id);
    }

    public Student AddStudent(Student student)
    {
        _lastIdDao.IncrementStudentId();
        student.Id = _lastIdDao.GetStudentId();
        Dictionary<string, Student> students = GetAllStudents();
        students[student.Id] = student;
        SaveStudents(students);
        return student;
    }

    public Student UpdateStudent(Student student)
    {
        Dictionary<string, Student> students = GetAllStudents();
        students[student.Id] = student;
        SaveStudents(students);
        return student;
    }

    public void DeleteStudent(string id)
    {
        Dictionary<string, Student> students = GetAllStudents();
        students.Remove(id);
        SaveStudents(students);
    }

    private void SaveStudents(Dictionary<string, Student> students)
    {
        JsonUtil.WriteToFile(students, Constants.StudentFilePath);
    }

}