using System;
using System.Collections.Generic;

namespace LangLang.Model;

[Flags]
public enum ValidationError
{
    None             = 0,
    FieldsEmpty      = 1,
    NameInvalid      = 2,
    SurnameInvalid   = 4,
    PasswordInvalid  = 8,
    PhoneInvalid     = 16,
    EmailInvalid     = 32,
    EmailUnavailable = 64
};

public static class ValidationErrorExtensions
{
    /// <summary>
    /// Returns all error messages, without punctation marks.
    /// </summary>
    public static List<string> GetAllMessages(this ValidationError error)
    {
        List<string> result = new();
        foreach (ValidationError flag in Enum.GetValues(typeof(ValidationError)))
        {
            if (flag == ValidationError.None)
                continue;
            if (error.HasFlag(flag))
                result.Add(flag.GetMessage());
        }
        return result;
    }
    /// <summary>
    /// Returns first error message, without punctation mark.
    /// </summary>
    public static string GetMessage(this ValidationError error) 
        => error switch
    {
        ValidationError.None => "Valid",
        { } when error.HasFlag(ValidationError.FieldsEmpty)
            => "All the fields are required",
        { } when error.HasFlag(ValidationError.NameInvalid)
            => "Name must be all letters",
        { } when error.HasFlag(ValidationError.SurnameInvalid)
            => "Surname must be all letters",
        { } when error.HasFlag(ValidationError.PhoneInvalid)
            => "Phone must be numerical, at least 6 long",
        { } when error.HasFlag(ValidationError.PasswordInvalid)
            => "Password must be 8+ chars with uppercase and num",
        { } when error.HasFlag(ValidationError.EmailInvalid)
            => "Email invalid",
        { } when error.HasFlag(ValidationError.EmailUnavailable)
            => "Email unavailable",
        _ => "User data invalid"
    };
    /// <summary>
    ///     Returns flag message if error has that flag, empty string otherwise.
    /// </summary>
    public static string GetMessageIfFlag(this ValidationError error, ValidationError flag)
        => error.HasFlag(flag) ? flag.GetMessage() : ""; 
}
