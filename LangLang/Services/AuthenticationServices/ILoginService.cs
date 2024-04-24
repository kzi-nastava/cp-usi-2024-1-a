namespace LangLang.Services.AuthenticationServices;

public interface ILoginService
{
    public LoginResult LogIn(string email, string password);

    public void LogOut();
}