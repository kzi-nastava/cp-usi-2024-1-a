using System;
using Consts;

namespace LangLang.Services.AuthenticationServices;

public interface IRegisterService
{
    public bool RegisterStudent(string? email, string? password, string? name, string? surname, DateTime birthDay,
        Gender gender, string? phoneNumber, EducationLvl educationLvl);

    public bool IsEmailTaken(string email);

    public bool CheckUserData(string email, string password, string name, string surname, string phoneNumber);

    public bool CheckPassword(string password);

    public bool CheckPhoneNumber(string phoneNumber);
}