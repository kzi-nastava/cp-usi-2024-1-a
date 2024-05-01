﻿using System.Collections.Generic;
using LangLang.Model;

namespace LangLang.DAO;

public interface IStudentDAO
{
    public Dictionary<string, Student> GetAllStudents();
    public Student? GetStudent(string id);
    public Student AddStudent(Student student);
    public void DeleteStudent(string id);
    
}