using LangLang.Application.DTO;

namespace LangLang.Application.UseCases.Authentication;

public interface ILoginService
{
    public LoginResult LogIn(string email, string password);

    public void LogOut();
}