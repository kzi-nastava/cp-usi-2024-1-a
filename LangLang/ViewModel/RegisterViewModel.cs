using System;
using System.Linq;
using System.Net.Mail;
using System.Windows;
using System.Windows.Input;
using Consts;
using LangLang.MVVM;
using LangLang.Services;
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

        public RegisterViewModel()
        {
            SignUpCommand = new RelayCommand(SignUp!);
        }


        public RegisterViewModel(RegisterView registerView)
        {
            _registerView = registerView;
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
<<<<<<< HEAD
            set
            {
                _educationLvl = value;
                OnPropertyChanged(nameof(EducationLvl));
            }
        }

        public string BirthdayFormatted => _birthday.ToString("yyyy-MM-dd");
=======
            set => SetField(ref _educationLvl, value);
        }
        public string? BirthdayFormatted => _birthday.ToString("yyyy-MM-dd");
>>>>>>> 162f87ce75176486c7ed25b989692bd6e1af3979
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

<<<<<<< HEAD
            bool successful = RegisterService.RegisterStudent(email, password, name, surname, birthday, gender, phoneNumber, educationLvl);
=======
            bool successful = RegisterService.RegisterStudent(email!, password!, name!, surname!, birthday, gender, phoneNumber!, educationLvl);
>>>>>>> 162f87ce75176486c7ed25b989692bd6e1af3979

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
<<<<<<< HEAD

                if (name.Any(char.IsDigit))
                {
                    ErrorMessageName = "Name must be all letters";
                }

                if (surname.Any(char.IsDigit))
                {
                    ErrorMessageSurname = "Surname must be all letters";
                }

                if (!int.TryParse(phoneNumber, out _) || phoneNumber.Length < 6)
                {
                    ErrorMessagePhone = "Numerical, 6 or more numbers";
                }
                if (password.Length < 8 || !password.Any(char.IsDigit) || !password.Any(char.IsUpper))
                {
                    ErrorMessagePassword = "At least 8 chars, uppercase and number ";
                }


=======
                if(name!.Any(char.IsDigit))
                {
                    ErrorMessageName = "Name must be all letters";
                }
                if(surname!.Any(char.IsDigit))
                {
                    ErrorMessageSurname = "Surname must be all letters";
                }
                if(!RegisterService.CheckPhoneNumber(phoneNumber!))
                {
                    ErrorMessagePhone = "Numerical, 6 or more numbers";
                }
                if(!RegisterService.CheckPassword(password!))
                {
                    ErrorMessagePassword = "At least 8 chars, uppercase and number ";
                }
>>>>>>> 162f87ce75176486c7ed25b989692bd6e1af3979
                if (email != null && RegisterService.CheckExistingEmail(email))
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
