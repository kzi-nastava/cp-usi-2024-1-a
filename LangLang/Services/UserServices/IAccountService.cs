using Consts;
using System;
using LangLang.DTO;
using LangLang.Model;

namespace LangLang.Services.UserServices;
public interface IAccountService
{
    public void UpdateStudent(string studentId, string password, string name, string surname, DateTime birthDate, Gender gender, string phoneNumber);
    public void DeleteStudent(Student student);
    public void DeactivateStudentAccount(Student student);
    void RegisterStudent(RegisterStudentDto registerDto);
    void RegisterTutor(RegisterTutorDto registerDto);
}

