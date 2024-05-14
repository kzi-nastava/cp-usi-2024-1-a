using System;
using LangLang.Domain.Model;

namespace LangLang.Application.DTO;

public class RegisterStudentDto : RegisterDto
{
    public EducationLevel EducationLevel { get; }

    public RegisterStudentDto(string email, string password, string name, string surname, DateTime birthDay, Gender gender, string phoneNumber, EducationLevel educationLvl)
    : base(email, password, name, surname, birthDay, gender, phoneNumber)
    {
        EducationLevel = educationLvl;
    }
}