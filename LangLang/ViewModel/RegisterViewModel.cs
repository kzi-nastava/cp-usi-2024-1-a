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

        private RegisterView? _registerView;
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
            set
            {
                _errorMessageRequired = value;
                OnPropertyChanged(nameof(ErrorMessageRequired));
            }
        }

        public string? ErrorMessageEmail
        {
            get => _errorMessageEmail;
            set
            {
                _errorMessageEmail = value;
                OnPropertyChanged(nameof(ErrorMessageEmail));
            }
        }

        public string? ErrorMessagePassword
        {
            get => _errorMessagePassword;
            set
            {
                _errorMessagePassword = value;
                OnPropertyChanged(nameof(ErrorMessagePassword));
            }
        }

        public string? ErrorMessageName
        {
            get => _errorMessageName;
            set
            {
                _errorMessageName = value;
                OnPropertyChanged(nameof(ErrorMessageName));
            }
        }

        public string? ErrorMessageSurname
        {
            get => _errorMessageSurname;
            set
            {
                _errorMessageSurname = value;
                OnPropertyChanged(nameof(ErrorMessageSurname));
            }
        }

        public string? ErrorMessagePhone
        {
            get => _errorMessagePhone;
            set
            {
                _errorMessagePhone = value;
                OnPropertyChanged(nameof(ErrorMessagePhone));
            }
        }

        public string? Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string? Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        
        public string? Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string? Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }

        public string? PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        public Gender Gender
        {
            get => _gender;
            set
            {
                _gender = value;
                OnPropertyChanged(nameof(Gender));
            }
        }

        public EducationLvl EducationLvl
        {
            get => _educationLvl;
            set
            {
                _educationLvl = value;
                OnPropertyChanged(nameof(EducationLvl));
            }
        }
        public string? BirthdayFormatted => _birthday.ToString("yyyy-MM-dd");
        public DateTime Birthday
        {
            get => _birthday;
            set
            {
                _birthday = value;
                OnPropertyChanged(nameof(Birthday));
            }
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

            bool successful = RegisterService.RegisterStudent(email!, password!, name!, surname!, birthday, gender, phoneNumber!, educationLvl);

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
                if(!RegisterService.CheckPhoneNumber(phoneNumber!))
                {
                    ErrorMessagePhone = "Numerical, 6 or more numbers";
                }
                if(!RegisterService.CheckPassword(password!))
                {
                    ErrorMessagePassword = "At least 8 chars, uppercase and number ";
                }
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
