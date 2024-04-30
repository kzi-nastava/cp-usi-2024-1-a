using System;
using System.Collections.Generic;
using Consts;
using LangLang.Model;

namespace LangLang.Services.UserServices;

public interface IStudentService
{
    public bool UpdateStudent(Student student, string password, string name, string surname, DateTime birthDate, Gender gender,
        string phoneNumber);
    public void DeleteAccount(Student student);
    
}