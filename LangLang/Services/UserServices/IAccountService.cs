using Consts;
using System;

namespace LangLang.Services.UserServices;
public interface IAccountService
{
    public void UpdateStudent(string studentId, string password, string name, string surname, DateTime birthDate, Gender gender, string phoneNumber);
    public void DeleteStudent(string studentId);
}

