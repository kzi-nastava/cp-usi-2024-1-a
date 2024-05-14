using System;
using LangLang.Domain.Model;

namespace LangLang.Application.Utility.Validators;

public interface IUserValidator
{
    ValidationError CheckUserData(string? email, string? password, string? name, string? surname, string? phoneNumber, DateTime? birthDay);

    /// <summary>
    ///     Return ValidationError.EmailUnavailable if email is taken.
    /// </summary>
    ValidationError EmailTaken(string email);
}