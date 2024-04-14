﻿using System;
using System.Linq;
using System.Net.Mail;
using System.Windows;
using System.Windows.Input;
using Consts;
using LangLang.MVVM;
using LangLang.Services;
using LangLang.Services.AuthenticationServices;
using LangLang.View;

namespace LangLang.ViewModel
{
    internal class RegisterViewModel : ViewModelBase
    {
        private string? _email;
        private string? _password;
        private string? _name;
        private string? _surname;
        private string? _phoneNumber;
        private Gender _gender;
        private DateTime _birthday;
        private EducationLvl _educationLvl;

        private string? _errorMessageRequired;
        private string? _errorMessageEmail;
        private string? _errorMessagePassword;
        private string? _errorMessageName;
        private string? _errorMessageSurname;
        private string? _errorMessagePhone;

        private readonly RegisterView? _registerView;
        public ICommand SignUpCommand { get; }

        private readonly IRegisterService _registerService;
        
        public RegisterViewModel(IRegisterService registerService)
        {
            SignUpCommand = new RelayCommand(SignUp!);
            _registerService = registerService;
        }


        public RegisterViewModel(RegisterView registerView, IRegisterService registerService)
        {
            _registerView = registerView;
            _registerService = registerService;
            SignUpCommand = new RelayCommand(SignUp!);
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

        public EducationLvl EducationLvl
        {
            get => _educationLvl;
            set => SetField(ref _educationLvl, value);
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
            EducationLvl educationLvl = EducationLvl;

            bool successful = _registerService.RegisterStudent(email, password, name, surname, birthday, gender, phoneNumber, educationLvl);


            if (!successful)
            {
                if(StudentAccountViewModel.AccountFieldsEmpty(birthday, password!, name!, surname!, phoneNumber!))
                {
                    ErrorMessageRequired = "All the fields are required";
                    return;
                }
                try
                {
                    _ = new MailAddress(email!);
                }
                catch
                {
                    ErrorMessageEmail = "Incorrect email";
                }
                if(name!.Any(char.IsDigit))
                {
                    ErrorMessageName = "Name must be all letters";
                }
                if(surname!.Any(char.IsDigit))
                {
                    ErrorMessageSurname = "Surname must be all letters";
                }
                if(!_registerService.CheckPhoneNumber(phoneNumber!))
                {
                    ErrorMessagePhone = "Numerical, 6 or more numbers";
                }
                if(!_registerService.CheckPassword(password!))
                {
                    ErrorMessagePassword = "At least 8 chars, uppercase and number ";
                }
                if (email != null && _registerService.CheckExistingEmail(email))
                {
                    ErrorMessageEmail = "Email already exists";
                }
            }
            else
            {
                MessageBox.Show($"Succesfull registration");
                LoginWindow view = new LoginWindow();
                view.Show();
                _registerView!.Close();
            }
        }
    }
}
