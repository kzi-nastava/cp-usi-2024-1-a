using System;
using System.Linq;
using System.Net.Mail;
using LangLang.Application.Utility.Authentication;
using LangLang.Domain.Model;

namespace LangLang.Application.Utility.Validators;

public class UserValidator : IUserValidator
{
    private readonly IProfileService _profileService;
    
    public UserValidator(IProfileService profileService)
    {
        _profileService = profileService;
    }

    public ValidationError EmailTaken(string? email)
    {
        if (email == null || email == "" || !_profileService.IsEmailTaken(email))
            return ValidationError.None;
        else
            return ValidationError.EmailUnavailable;
    }

    public ValidationError CheckUserData(string? email, string? password, string? name, string? surname, string? phoneNumber, DateTime? birthDay)
    {
        if (FieldsEmpty(email, password, name, surname, phoneNumber, birthDay))
            return ValidationError.FieldsEmpty;

        ValidationError error = ValidationError.None;
        error |= CheckName(name!, surname!);
        error |= CheckPassword(password!);
        error |= CheckPhoneNumber(phoneNumber!);
        try
        {
            _ = new MailAddress(email!);
        }
        catch
        {
            error |= ValidationError.EmailInvalid;
        }
        return error;
    }

    private bool FieldsEmpty(string? email, string? password, string? name, string? surname, string? phoneNumber, DateTime? birthDate)
        => email == null || password == null || name == null || surname == null || phoneNumber == null 
        || email == ""   || password == ""   || name == ""   || surname == ""   || phoneNumber == ""
        || birthDate == null || birthDate == DateTime.MinValue;

    private ValidationError CheckPassword(string password)
    {
        if (password.Length >= 8 && password.Any(char.IsDigit) && password.Any(char.IsUpper))
            return ValidationError.None;
        else
            return ValidationError.PasswordInvalid;
    }

    private ValidationError CheckPhoneNumber(string phoneNumber)
    {
        foreach (char c in phoneNumber)
        {
            if (c < '0' || c > '9')
                return ValidationError.PhoneInvalid;
        }
        if (phoneNumber.Length < 6)
            return ValidationError.PhoneInvalid;
        else
            return ValidationError.None;
    }

    private ValidationError CheckName(string name, string surname)
    {
        ValidationError error = ValidationError.None;
        if (name.Any(char.IsDigit))
            error |= ValidationError.NameInvalid;
        if (surname.Any(char.IsDigit))
            error |= ValidationError.SurnameInvalid;
        return error;
    }
}
