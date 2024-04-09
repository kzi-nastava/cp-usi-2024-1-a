using System;
using Consts;
using System.Collections.Generic;
using LangLang.Model;
using LangLang.Util;
using LangLang.Services;

public class StudentDAO
{
    private static StudentDAO? instance;
    private Dictionary<string, Student>? students;

    private Dictionary<string, Student> Students
    {
        get {
            students ??= JsonUtil.ReadFromFile<Student>(Constants.StudentFilePath);
            return students!;
        }
        set => students = value;
    }

    //Singleton
	private StudentDAO()
	{
	}

    public static StudentDAO GetInstance()
    {
        return instance ??= new StudentDAO();
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
        students![student.Email] = student;
        SaveStudents();
        return student;
    }

    public void DeleteStudent(string email)
    {
        students!.Remove(email);
        SaveStudents();
    }

    public void SaveStudents()
    {
        JsonUtil.WriteToFile(Students, Constants.StudentFilePath);
    }

}
