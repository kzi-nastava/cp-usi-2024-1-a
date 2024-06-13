using System;
using LangLang.Application.DTO;
using LangLang.Application.Stores;
using LangLang.Application.UseCases.Authentication;
using LangLang.CLI.Util;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;

namespace LangLang.CLI.Views;

public class LoginMenu : ICliMenu
{
    private readonly ILoginService _loginService;
    private readonly IAuthenticationStore _authenticationStore;

    private readonly TutorMenu _tutorMenu;
    private readonly DirectorMenu _directorMenu;

    public LoginMenu(ILoginService loginService, TutorMenu tutorMenu, IAuthenticationStore authenticationStore, DirectorMenu directorMenu)
    {
        _loginService = loginService;
        _tutorMenu = tutorMenu;
        _authenticationStore = authenticationStore;
        _directorMenu = directorMenu;
    }

    public void Show()
    {
        LoginResult loginResult;
        
        Console.Clear();
        while (true)
        {
            Console.WriteLine("=== Login ===");
            
            var email = InputHandler.ReadString("Enter your email: ") ?? "";
            var password = InputHandler.ReadSecretString("Enter your password: ");

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
                _directorMenu.Show();
                break;
            case UserType.Tutor:
                _tutorMenu.LoggedInTutor = (Tutor?)_authenticationStore.CurrentUser.Person;
                _tutorMenu.Show();
                break;
            default:
                Console.WriteLine("CLI for this user type is not supported yet.");
                break;
        }
        
    }
}