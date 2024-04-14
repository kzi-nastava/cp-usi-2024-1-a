using Consts;
using LangLang.Model;
using LangLang.MVVM;
using LangLang.Services;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using LangLang.Services.AuthenticationServices;
using LangLang.Services.UserServices;

namespace LangLang.ViewModel
{
    internal class StudentAccountViewModel : ViewModelBase
    {
        private StudentService studentService = StudentService.GetInstance();
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

        private IRegisterService _registerService;
        
        public StudentAccountViewModel(IRegisterService registerService)
        {
            _registerService = registerService;
            ConfirmCommand = new RelayCommand(ConfirmInput!);
            SetUserData();
        }

        private void SetUserData()
        {
            Student user = studentService.LoggedUser!;
            Email = user.Email;
            Name = user.Name;
            Surname = user.Surname;
            PhoneNumber = user.PhoneNumber;
            Gender = user.Gender;
            Birthday = user.BirthDate;
            Password = user.Password; 
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

            bool successful = _registerService.CheckUserData(studentService.LoggedUser!.Email, password, name, surname, phoneNumber);

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
                if (AccountFieldsEmpty(birthday, password, name, surname, phoneNumber)) 
                {
                    ErrorMessageRequired = "All the fields are required";
                    return;
                }

                if (name.Any(char.IsDigit))
                {
                    ErrorMessageName = "Name must be all letters";
                }

                if (surname.Any(char.IsDigit))
                {
                    ErrorMessageSurname = "Surname must be all letters";
                }

                if (!_registerService.CheckPhoneNumber(phoneNumber))
                {
                    ErrorMessagePhone = "Must be made up of numbers";
                }
                if (!_registerService.CheckPassword(password))
                {
                    ErrorMessagePassword = "At least 8 chars, uppercase and number ";
                }
            }
        }
        public static bool AccountFieldsEmpty(DateTime birthday,  string password, string name, string surname, string phoneNumber)
        {
            return birthday == DateTime.MinValue || password == null || name == null || surname == null || phoneNumber == null || name == "" || surname == "" || password == "" || phoneNumber == "";
        }

    }
}
