using Consts;
using System;
using LangLang.DTO;

namespace LangLang.Services.UserServices;
public interface IAccountService
{
    public void UpdateStudent(string studentId, string password, string name, string surname, DateTime birthDate, Gender gender, string phoneNumber);
    public void DeleteStudent(string studentId);
    void RegisterStudent(RegisterStudentDto registerDto);
    void RegisterTutor(RegisterTutorDto registerDto);
}

