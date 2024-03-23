using System;
using Consts;
using System.Collections.Generic;

public class StudentDAO
{
    private static StudentDAO instance;
    Dictionary<string, Student> students;


	private StudentDAO()
	{
	}

    public static StudentDAO GetInstance()
    {
        if (instance == null)
        {
            instance = new StudentDAO();
        }
        return instance;
    }

    public Dictionary<string, Student> GetAllStudents()
	{
        if (students == null)
        {
            students = JsonUtil.loadAllStudents(Constants.StudentFilePath);
        }
		return students;
    }

	public Student FindStudent(string email)
	{
		return students[email];
	}

}
