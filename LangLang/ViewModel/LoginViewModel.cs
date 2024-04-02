using System;
using System.ComponentModel;
using System.Security;
using System.Windows;
using System.Windows.Input;
using LangLang.MVVM;
using LangLang.View;
using LangLang.Model;
using LangLang.Services;

namespace LangLang.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _email;
        private SecureString _password;
        private string _errorMessage;

        private readonly LoginService _loginService;


        private readonly Window _window;

        public LoginViewModel()
        {
            _loginService = LoginService.GetInstance();
            LoginCommand = new RelayCommand(Login);
        }

        public LoginViewModel(Window window)
        {
            _window = window;
            _loginService = LoginService.GetInstance();
            LoginCommand = new RelayCommand(Login);
        }




        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public SecureString Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public ICommand LoginCommand { get; }

        private void Login(object parameter)
        {
            ErrorMessage = "";

            // Directly access the Email and Password properties
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
                }
                else
                {
                    loggedUser = StudentService.GetInstance().LoggedUser;
                    StudentWindow studentWindow = new StudentWindow();
                    studentWindow.Show();
                }
                _window.Close();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Helper method to convert SecureString to plain text
        private string ConvertToUnsecureString(SecureString securePassword)
        {
            if (securePassword == null)
                return string.Empty;

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = System.Runtime.InteropServices.Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return System.Runtime.InteropServices.Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

    }
}