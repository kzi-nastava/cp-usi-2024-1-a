using System;
using System.Windows;
using System.Windows.Input;
using LangLang.Application.DTO;
using LangLang.Application.Stores;
using LangLang.Application.UseCases.Authentication;
using LangLang.Application.Utility.Navigation;
using LangLang.Application.Utility.Validators;
using LangLang.Domain.Model;
using LangLang.WPF.MVVM;
using LangLang.WPF.ViewModels.Factories;

namespace LangLang.WPF.ViewModels.Common
{
    public class RegisterViewModel : ViewModelBase, INavigableDataContext
    {
        private string? _email;
        private string? _password;
        private string? _name;
        private string? _surname;
        private string? _phoneNumber;
        private Gender _gender;
        private DateTime _birthday;
        private EducationLevel _educationLevel;

        private string? _errorMessageRequired;
        private string? _errorMessageEmail;
        private string? _errorMessagePassword;
        private string? _errorMessageName;
        private string? _errorMessageSurname;
        private string? _errorMessagePhone;
        
        public ICommand SignUpCommand { get; }
        public ICommand SwitchToLoginCommand { get; }

        private readonly IRegisterService _registerService;
        private readonly ILoginService _loginService;
        private readonly IUserValidator _userValidator;
        private readonly INavigationService _navigationService;
        
        public NavigationStore NavigationStore { get; }
        
        public RegisterViewModel(IRegisterService registerService, ILoginService loginService, IUserValidator userValidator, INavigationService navigationService, NavigationStore navigationStore)
        {
            _registerService = registerService;
            _loginService = loginService;
            _userValidator = userValidator;
            _navigationService = navigationService;
            NavigationStore = navigationStore;
            SignUpCommand = new RelayCommand(SignUp!);
            SwitchToLoginCommand = new RelayCommand(SwitchToLogin);
        }

        public string? ErrorMessageRequired
        {
            get => _errorMessageRequired;
            set => SetField(ref _errorMessageRequired, value);
        }

        public string? ErrorMessageEmail
        {
            get => _errorMessageEmail;
            set => SetField(ref _errorMessageEmail, value);
        }

        public string? ErrorMessagePassword
        {
            get => _errorMessagePassword;
            set => SetField(ref _errorMessagePassword, value);
        }

        public string? ErrorMessageName
        {
            get => _errorMessageName;
            set => SetField(ref _errorMessageName, value);
        }

        public string? ErrorMessageSurname
        {
            get => _errorMessageSurname;
            set => SetField(ref _errorMessageSurname, value);
        }

        public string? ErrorMessagePhone
        {
            get => _errorMessagePhone;
            set => SetField(ref _errorMessagePhone, value);
        }

        public string? Email
        {
            get => _email;
            set => SetField(ref _email, value);
        }

        public string? Password
        {
            get => _password;
            set => SetField(ref _password, value);
        }
        
        public string? Name
        {
            get => _name;
            set => SetField(ref _name, value);
        }
        public string? Surname
        {
            get => _surname;
            set => SetField(ref _surname, value);
        }

        public string? PhoneNumber
        {
            get => _phoneNumber;
            set => SetField(ref _phoneNumber, value);
        }

        public Gender Gender
        {
            get => _gender;
            set => SetField(ref _gender, value);
        }

        public EducationLevel EducationLevel
        {
            get => _educationLevel;
            set => SetField(ref _educationLevel, value);
        }
        public string? BirthdayFormatted => _birthday.ToString("yyyy-MM-dd");

        public DateTime Birthday
        {
            get => _birthday;
            set => SetField(ref _birthday, value);
        }

        private void SignUp(object parameter)
        {
            ErrorMessageRequired = "";
            ErrorMessageEmail = "";
            ErrorMessagePassword = "";
            ErrorMessageName = "";
            ErrorMessageSurname = "";
            ErrorMessagePhone = "";

            // Directly access the properties
            string? email = Email;
            string? password = Password;
            string? name = Name;
            string? surname = Surname;
            string? phoneNumber = PhoneNumber;
            Gender gender = Gender;
            DateTime birthday = Birthday;
            EducationLevel educationLevel = EducationLevel;

            ValidationError error = _registerService.RegisterStudent(email, password, name, surname, birthday, gender, phoneNumber, educationLevel);


            if (error != ValidationError.None)
            {
                if (error.HasFlag(ValidationError.FieldsEmpty))
                {
                    ErrorMessageRequired = ValidationError.FieldsEmpty.GetMessage();
                    return;
                }

                ErrorMessageEmail = error.GetMessageIfFlag(ValidationError.EmailInvalid);
                ErrorMessageName = error.GetMessageIfFlag(ValidationError.NameInvalid);
                ErrorMessageSurname = error.GetMessageIfFlag(ValidationError.SurnameInvalid);
                ErrorMessagePhone = error.GetMessageIfFlag(ValidationError.PhoneInvalid);
                ErrorMessagePassword = error.GetMessageIfFlag(ValidationError.PasswordInvalid);

                if (_userValidator.EmailTaken(email!) != ValidationError.None)
                    ErrorMessageEmail = ValidationError.EmailUnavailable.GetMessage();
            }
            else
            {
                MessageBox.Show($"Succesfull registration");
                SwitchToLogin(null);
            }
        }

        private void SwitchToLogin(object? parameter)
        {
            _navigationService.Navigate(ViewType.Login);
        }
    }
}
