using System;
using Consts;
using System.Collections.Generic;
using LangLang.Model;

public class StudentDAO
{
    private static StudentDAO instance;
    Dictionary<string, Student> students;

    //Singleton
	private StudentDAO()
	{
	}

    public static StudentDAO GetInstance()
    {
        if (instance == null)
        {
            instance = new StudentDAO();
            instance.GetAllStudents();
        }
        return instance;
    }

    public Dictionary<string, Student> GetAllStudents()
	{
        if (students == null)
        {
            students = JsonUtil.ReadFromFile<Student>(Constants.StudentFilePath);
        }
		return students;
    }

	public Student FindStudent(string email)
	{
        if (students.TryGetValue(email, out Student student))
        {
            return student;
        }
        else
        {
            return null;
        }
    }

    public void AddStudent(Student student)
    {
        students[student.Email] = student;
        JsonUtil.WriteToFile(students, Constants.StudentFilePath);
    }

    public void DeleteStudent(string email)
    {
        students.Remove(email);
    }


}
