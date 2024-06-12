using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Documents;
using System.Windows.Input;
using LangLang.Application.DTO;
using LangLang.Application.Stores;
using LangLang.Application.UseCases.Authentication;
using LangLang.Application.UseCases.Report;
using LangLang.Application.Utility.Navigation;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;
using LangLang.Repositories.Json;
using LangLang.Repositories.SQL;
using LangLang.WPF.MVVM;
using LangLang.WPF.ViewModels.Factories;

namespace LangLang.WPF.ViewModels.Common
{
    public class LoginViewModel : ViewModelBase, INavigableDataContext
    {
        private string? _email;
        private SecureString? _password;
        private string? _errorMessage;
        
        private readonly ILoginService _loginService;
        private readonly INavigationService _navigationService;
        
        public NavigationStore NavigationStore { get; }

        public ICommand LoginCommand { get; }
        public ICommand SwitchToRegisterCommand { get; }

        public LoginViewModel(ILoginService loginService, INavigationService navigationService, NavigationStore navigationStore, ICourseRepositorySQL crs)
        {
            _loginService = loginService;
            _navigationService = navigationService;
            NavigationStore = navigationStore;
            LoginCommand = new RelayCommand(Login!);
            SwitchToRegisterCommand = new RelayCommand(SwitchToRegister);
            //ICourseRepositorySQL crs = new CourseRepositorySQL(new DatabaseCredentials("localhost", 5433, "postgres", "123", "langlang"));
            //"Host=localhost;,Username=postgres;Password=your_password;Database=langlang";

            //List<Course> cccc = crs.GetAll();
            //int b = 9;
            //Course course = crs.Get("18");
            CourseRepository cr = new CourseRepository(LangLang.Repositories.Constants.CourseFilePath, LangLang.Repositories.Constants.CourseIdFilePath);
            /*List<Course> courses = cr.GetAll();
            int i = 0;
            foreach (Course course in courses)
            {
                crs.Add(course);
            }*/
            List<string> ids = new List<string>();
            ids.Add("19");
            ids.Add("22");
            List<Course> lista = crs.Get(ids);
     
            crs.Delete("17");



            int k = 3;
        }

        public string Email
        {
            get => _email!;
            set => SetField(ref _email, value);
        }

        public SecureString Password
        {
            get => _password!;
            set => SetField(ref _password, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage!;
            set =>SetField(ref _errorMessage, value);
        }
        
        private void Login(object parameter)
        {
            ErrorMessage = "";
            string email = Email;
            string password = ConvertToUnsecureString(Password);
            LoginResult loginResult = _loginService.LogIn(email, password);

            //LoginFailed
            if (!loginResult.IsValidUser)
            {
                if (!loginResult.IsValidEmail)
                {
                    ErrorMessage = "User doesn't exist";
                }
                else
                {
                    ErrorMessage = "Incorrect password";
                }
            }
            else
            {
                switch (loginResult.UserType)
                {
                    case UserType.Director:
                        _navigationService.Navigate(ViewType.Director);
                        break;
                    case UserType.Tutor:
                        _navigationService.Navigate(ViewType.Tutor);
                        break;
                    case UserType.Student:
                        _navigationService.Navigate(ViewType.Student);
                        break;
                    default:
                        throw new ArgumentException("No available window for current user type");
                }
            }
        }

        private void SwitchToRegister(object? parameter)
        {
            _navigationService.Navigate(ViewType.Register);
        }

        // Helper method to convert SecureString to plain text
        private static string ConvertToUnsecureString(SecureString securePassword)
        {
            if (securePassword == null)
                return string.Empty;

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString)!;
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

    }
}