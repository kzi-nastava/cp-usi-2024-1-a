namespace LangLang.Services.AuthenticationServices;

public interface ILoginService
{
    public void LogIn(string email, string password);
}