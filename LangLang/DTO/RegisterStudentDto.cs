using System;
using Consts;

namespace LangLang.DTO;

public class RegisterStudentDto : RegisterDto
{
    public EducationLvl EducationLvl { get; }

    public RegisterStudentDto(string email, string password, string name, string surname, DateTime birthDay, Gender gender, string phoneNumber, EducationLvl educationLvl)
    : base(email, password, name, surname, birthDay, gender, phoneNumber)
    {
        EducationLvl = educationLvl;
    }
}