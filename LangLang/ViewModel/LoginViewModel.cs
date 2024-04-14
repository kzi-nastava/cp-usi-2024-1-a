using System;
using System.Security;
using System.Windows;
using System.Windows.Input;
using LangLang.MVVM;
using LangLang.View;
using LangLang.Model;
using LangLang.Services;
using LangLang.Services.AuthenticationServices;
using LangLang.Services.UserServices;

namespace LangLang.ViewModel
{
    internal class LoginViewModel : ViewModelBase
    {
        private string? _email;
        private SecureString? _password;
        private string? _errorMessage;
        private readonly LoginService _loginService;
        private readonly Window? _window;

        public LoginViewModel()
        {
            _loginService = LoginService.GetInstance();
            LoginCommand = new RelayCommand(Login!);
        }

        public LoginViewModel(Window window)
        {
            _window = window;
            _loginService = LoginService.GetInstance();
            LoginCommand = new RelayCommand(Login!);
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

        public ICommand LoginCommand { get; }

        private void Login(object parameter)
        {
            ErrorMessage = "";
            string email = Email;
            string password = ConvertToUnsecureString(Password);
            _loginService.LogIn(email, password);

            //LoginFailed
            if (!_loginService.validUser)
            {
                if (!_loginService.validEmail)
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
                User loggedUser;
                if (_loginService.userType == typeof(Director))
		        {
                    loggedUser = DirectorService.GetInstance().LoggedUser;
                    DirectorWindow directorWindow = new DirectorWindow();
                    directorWindow.Show();
                }
		        else if (_loginService.userType == typeof(Tutor))
                {
		            loggedUser = TutorService.GetInstance().LoggedUser;
                    TutorWindow tutorWindow = new TutorWindow
                    {
                        DataContext = new TutorViewModel((Tutor)loggedUser)
                    };
                    tutorWindow.Show();
                }
		        else
                {
		            loggedUser = StudentService.GetInstance().LoggedUser!;
                    StudentWindow studentWindow = new StudentWindow();
                    studentWindow.Show();
		        }
                _window!.Close();
            }
        }

        // Helper method to convert SecureString to plain text
        private static string ConvertToUnsecureString(SecureString securePassword)
        {
            if (securePassword == null)
                return string.Empty;

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = System.Runtime.InteropServices.Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return System.Runtime.InteropServices.Marshal.PtrToStringUni(unmanagedString)!;
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

    }
}