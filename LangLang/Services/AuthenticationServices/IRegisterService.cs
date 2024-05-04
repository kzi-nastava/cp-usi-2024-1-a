using System;
using LangLang.Model;
using Consts;

namespace LangLang.Services.AuthenticationServices;

public interface IRegisterService
{
    public ValidationError RegisterStudent(string? email, string? password, string? name, string? surname, DateTime birthDay,
        Gender gender, string? phoneNumber, EducationLvl educationLvl);
}