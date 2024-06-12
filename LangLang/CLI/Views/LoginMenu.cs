using System;
using LangLang.Application.DTO;
using LangLang.Application.UseCases.Authentication;
using LangLang.CLI.Util;
using LangLang.Domain.Model;

namespace LangLang.CLI.Views;

public class LoginMenu : ICliMenu
{
    private readonly ILoginService _loginService;

    private readonly TutorMenu _tutorMenu;

    public LoginMenu(ILoginService loginService, TutorMenu tutorMenu)
    {
        _loginService = loginService;
        _tutorMenu = tutorMenu;
    }

    public void Show()
    {
        LoginResult loginResult;
        
        Console.Clear();
        while (true)
        {
            Console.WriteLine("=== Login ===");
            
            var email = InputHandler.ReadString("Enter your email: ") ?? "";
            var password = InputHandler.ReadSecretString("Enter your username: ");

            loginResult = _loginService.LogIn(email, password);
            if (loginResult.IsValidUser)
            {
                break;
            }
            Console.WriteLine(!loginResult.IsValidEmail ? "User does not exist." : "Incorrect password");
            Console.WriteLine();
        }
        
        switch (loginResult.UserType)
        {
            case UserType.Director:
                        
                break;
            case UserType.Tutor:
                _tutorMenu.Show();
                break;
            default:
                Console.WriteLine("CLI for this user type is not supported yet.");
                break;
        }
        
    }
}