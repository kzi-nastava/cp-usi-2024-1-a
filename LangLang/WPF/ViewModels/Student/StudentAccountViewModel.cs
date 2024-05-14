using System;
using System.Windows;
using System.Windows.Input;
using LangLang.Application.Stores;
using LangLang.Application.UseCases.User;
using LangLang.Application.Utility.Validators;
using LangLang.Domain.Model;
using LangLang.WPF.MVVM;

namespace LangLang.WPF.ViewModels.Student
{
    public class StudentAccountViewModel : ViewModelBase, INavigableDataContext
    {
        private readonly IAccountService _accountService;
        private readonly IAuthenticationStore _authenticationStore;
        public NavigationStore NavigationStore { get; }
        
        private string? _email;
        private string? _password;
        private string? _name;
        private string? _surname;
        private string? _phoneNumber;
        private Gender _gender;
        private DateTime _birthday;

        private string? _errorMessageRequired;
        private string? _errorMessagePassword;
        private string? _errorMessageName;
        private string? _errorMessageSurname;
        private string? _errorMessagePhone;

        private readonly IUserValidator _userValidator;

        private Domain.Model.Student user = null!;
        
        public StudentAccountViewModel(IAccountService accountService,IUserValidator userValidator, IAuthenticationStore authenticationStore, NavigationStore navigationStore)
        {
            _accountService = accountService;
            _userValidator = userValidator;
            _authenticationStore = authenticationStore;
            NavigationStore = navigationStore;
            ConfirmCommand = new RelayCommand(ConfirmInput!);
            SetUserData();
        }

        private void SetUserData()
        {
            Profile profile = _authenticationStore.CurrentUserProfile ?? 
                              throw new InvalidOperationException("Cannot set user data without currently logged in user");
            user = (Domain.Model.Student?)_authenticationStore.CurrentUser.Person ??
                        throw new InvalidOperationException("Cannot set user data without currently logged in user");
            Email = profile.Email;
            Name = user.Name;
            Surname = user.Surname;
            PhoneNumber = user.PhoneNumber;
            Gender = user.Gender;
            Birthday = user.BirthDate;
            Password = profile.Password; 
        }

        public string ErrorMessageRequired
        {
            get => _errorMessageRequired!;
            set => SetField(ref _errorMessageRequired, value);
        }

        public string ErrorMessagePassword
        {
            get => _errorMessagePassword!;
            set => SetField(ref _errorMessagePassword, value);
        }

        public string ErrorMessageName
        {
            get => _errorMessageName!;
            set => SetField(ref _errorMessageName, value);
        }

        public string ErrorMessageSurname
        {
            get => _errorMessageSurname!;
            set => SetField(ref _errorMessageSurname, value);
        }

        public string ErrorMessagePhone
        {
            get => _errorMessagePhone!;
            set => SetField(ref _errorMessagePhone, value);
        }

        public string Email
        {
            get => _email!;
            set => SetField(ref _email, value);
        }

        public string Password
        {
            get => _password!;
            set => SetField(ref _password, value);
        }

        public string Name
        {
            get => _name!;
            set => SetField(ref _name, value);
        }

        public string Surname
        {
            get => _surname!;
            set => SetField(ref _surname, value);
        }

        public string PhoneNumber
        {
            get => _phoneNumber!;
            set => SetField(ref _phoneNumber, value);
        }
        public Gender Gender
        {
            get => _gender;
            set => SetField(ref _gender, value);
        }
        public string BirthdayFormatted => _birthday.ToString("yyyy-MM-dd");
        public DateTime Birthday
        {
            get => _birthday;
            set => SetField(ref _birthday, value);
        }

        public ICommand ConfirmCommand { get; }

        private void ConfirmInput(object parameter)
        {
            ErrorMessageRequired = "";
            ErrorMessagePassword = "";
            ErrorMessageName = "";
            ErrorMessageSurname = "";
            ErrorMessagePhone = "";

            // Directly access the properties
            string email = Email;
            string password = Password;
            string name = Name;
            string surname = Surname;
            string phoneNumber = PhoneNumber;
            Gender gender = Gender;
            DateTime birthday = Birthday;

            ValidationError error = _userValidator.CheckUserData(email, password, name, surname, phoneNumber, birthday);

            if (error == ValidationError.None)
            {
                try
                {
                    _accountService.UpdateStudent(user.Id, password, name, surname, birthday, gender, phoneNumber);
                    MessageBox.Show($"Succesfull update");
                }
                catch {
                    ErrorMessageRequired = "Student applied for courses, editing profile not allowed";
                    return;
                }
            }
            else {
                if (error.HasFlag(ValidationError.FieldsEmpty)) 
                {
                    ErrorMessageRequired = ValidationError.FieldsEmpty.GetMessage();
                    return;
                }

                ErrorMessageName     = error.GetMessageIfFlag(ValidationError.NameInvalid);
                ErrorMessageSurname  = error.GetMessageIfFlag(ValidationError.SurnameInvalid);
                ErrorMessagePhone    = error.GetMessageIfFlag(ValidationError.PhoneInvalid);
                ErrorMessagePassword = error.GetMessageIfFlag(ValidationError.PasswordInvalid);
            }
        }
    }
}
