using LangLang.Model;
using System;

namespace LangLang.Services.UtilityServices;

public interface IUserValidator
{
    ValidationError CheckUserData(string? email, string? password, string? name, string? surname, string? phoneNumber, DateTime? birthDay);
    ValidationError EmailTaken(string email);
}