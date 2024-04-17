using System;
using System.Security;
using System.Windows.Input;
using LangLang.MVVM;
using LangLang.Services.AuthenticationServices;
using LangLang.Services.NavigationServices;
using LangLang.Services.UtilityServices;
using LangLang.Stores;
using LangLang.ViewModel.Factories;

namespace LangLang.ViewModel
{
    public class LoginViewModel : ViewModelBase, INavigableDataContext
    {
        private string? _email;
        private SecureString? _password;
        private string? _errorMessage;
        
        private readonly ILoginService _loginService;
        private readonly INavigationService _navigationService;
        
        public NavigationStore NavigationStore { get; }

        public LoginViewModel(ILoginService loginService, INavigationService navigationService, NavigationStore navigationStore)
        {
            _loginService = loginService;
            _navigationService = navigationService;
            NavigationStore = navigationStore;
            LoginCommand = new RelayCommand(Login!);
            SwitchToRegisterCommand = new RelayCommand(SwitchToRegister);
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

        public ICommand SwitchToRegisterCommand { get; }

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