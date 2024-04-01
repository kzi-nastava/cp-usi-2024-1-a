using Consts;
using LangLang.Model;
using LangLang.MVVM;
using LangLang.Services;
using LangLang.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LangLang.ViewModel
{
    internal class StudentAccountViewModel : ViewModelBase
    {
        private StudentService studentService = StudentService.GetInstance();

        private string _email;
        private string _password;
        private string _name;
        private string _surname;
        private string _phoneNumber;
        private Gender _gender;
        private DateTime _birthday;

        private string _errorMessageRequired;
        private string _errorMessagePassword;
        private string _errorMessageName;
        private string _errorMessageSurname;
        private string _errorMessagePhone;


        private readonly Window _window;


        public StudentAccountViewModel()
        {
            ConfirmCommand = new RelayCommand(ConfirmInput);
        }


        private StudentAccountWindow _accountWindow;

        public StudentAccountViewModel(StudentAccountWindow accountWindow)
        {
            _accountWindow = accountWindow;
            ConfirmCommand = new RelayCommand(ConfirmInput);
            SetUserData();
        }


        private void SetUserData()
        {
            Student user = studentService.LoggedUser;
            Email = user.Email;
            Name = user.Name;
            Surname = user.Surname;
            PhoneNumber = user.PhoneNumber;
            Gender = user.Gender;
            Birthday = user.BirthDate;
            Password = user.Password; // Assuming you don't want to display the password
        }

        public string ErrorMessageRequired
        {
            get => _errorMessageRequired;
            set
            {
                _errorMessageRequired = value;
                OnPropertyChanged(nameof(ErrorMessageRequired));
            }
        }


        public string ErrorMessagePassword
        {
            get => _errorMessagePassword;
            set
            {
                _errorMessagePassword = value;
                OnPropertyChanged(nameof(ErrorMessagePassword));
            }
        }

        public string ErrorMessageName
        {
            get => _errorMessageName;
            set
            {
                _errorMessageName = value;
                OnPropertyChanged(nameof(ErrorMessageName));
            }
        }

        public string ErrorMessageSurname
        {
            get => _errorMessageSurname;
            set
            {
                _errorMessageSurname = value;
                OnPropertyChanged(nameof(ErrorMessageSurname));
            }
        }

        public string ErrorMessagePhone
        {
            get => _errorMessagePhone;
            set
            {
                _errorMessagePhone = value;
                OnPropertyChanged(nameof(ErrorMessagePhone));
            }
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

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }

        public string PhoneNumber
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
        public string BirthdayFormatted => _birthday.ToString("yyyy-MM-dd");
        public DateTime Birthday
        {
            get => _birthday;
            set
            {
                _birthday = value;
                OnPropertyChanged(nameof(Birthday));
            }
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

            bool successful = RegisterService.CheckUserData(studentService.LoggedUser.Email, password, name, surname, phoneNumber);

            if (successful)
            {
                bool canEdit = studentService.UpdateStudent(password, name, surname,birthday, gender, phoneNumber);
                if (canEdit)
                {
                    MessageBox.Show($"Succesfull update");
                }
                else
                {
                    ErrorMessageRequired = "Student applied for courses, editing profile not allowed";
                    return;
                }

            }
            else {
                if (birthday == DateTime.MinValue || password == null || name == null || surname == null || phoneNumber == null || name == "" || surname == "" || password == "" || phoneNumber == "")
                {
                    ErrorMessageRequired = "All the fields are required";
                    return;
                }

                if (int.TryParse(name, out _))
                {
                    ErrorMessageName = "Name must be all letters";
                }

                if (int.TryParse(surname, out _))
                {
                    ErrorMessageSurname = "Surname must be all letters";
                }

                if (!int.TryParse(phoneNumber, out _))
                {
                    ErrorMessagePhone = "Must be made up of numbers";
                }
                if (password.Length < 8 || !password.Any(char.IsDigit) || !password.Any(char.IsUpper))
                {
                    ErrorMessagePassword = "At least 8 chars, uppercase and number ";
                }

            }

        }
    }
}
