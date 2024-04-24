using LangLang.Model;

namespace LangLang.Services.AuthenticationServices;

public enum UserType
{
    Student, Tutor, Director
}

public class LoginResult
{
    public bool IsValidUser { get; }
    public User? User { get; }
    public UserType? UserType { get; }
    
    public bool IsValidEmail { get; }

    public LoginResult(bool isValidUser, bool isValidEmail = false, User? user = null, UserType? userType = null)
    {
        IsValidUser = isValidUser;
        IsValidEmail = isValidEmail;
        User = user;
        UserType = userType;
    }
}