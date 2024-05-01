using System;
using System.Linq;
using System.Net.Mail;
using Consts;
using LangLang.DTO;
using LangLang.Services.UserServices;

namespace LangLang.Services.AuthenticationServices
{
    public class RegisterService : IRegisterService
    {
        private readonly IProfileService _profileService;
        private readonly IAccountService _accountService;

        public RegisterService(IProfileService profileService, IAccountService accountService)
        {
            _profileService = profileService;
            _accountService = accountService;
        }

        public bool RegisterStudent(string? email, string? password, string? name, string? surname, DateTime birthDay, Gender gender, string? phoneNumber, EducationLvl educationLvl)
        {
            bool passed = CheckUserData(email, password, name, surname, phoneNumber);
            passed &= !IsEmailTaken(email!);
            passed &= (birthDay != DateTime.MinValue);

            if (passed)
            {
                _accountService.RegisterStudent(new RegisterStudentDto(
                    email!,
                    password!,
                    name!,
                    surname!,
                    birthDay,
                    gender,
                    phoneNumber!,
                    educationLvl
                    ));
            }
            return passed;
        }

        public bool IsEmailTaken(string email)
        {
            return _profileService.IsEmailTaken(email);
        }

        public bool CheckUserData(string? email, string? password, string? name, string? surname, string? phoneNumber)
        {
            if(FieldsEmpty(email, password, name, surname, phoneNumber))
            {
                return false;
            }
            bool passed = true;
            passed &= CheckName(name!, surname!);
            passed &= CheckPassword(password!);
            passed &= CheckPhoneNumber(phoneNumber!);
            try
            {
                _ = new MailAddress(email!);
            }
            catch
            {
                passed = false;
            }
            return passed;
        }

        private bool FieldsEmpty(string? email, string? password, string? name, string? surname, string? phoneNumber)
        {
            return email == null || password == null || name == null || surname == null || phoneNumber == null;
        }


        public bool CheckPassword(string password)
        {
            return password.Length >= 8 && password.Any(char.IsDigit) && password.Any(char.IsUpper);
        }

        public bool CheckPhoneNumber(string phoneNumber)
        {
            foreach (char c in phoneNumber)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return phoneNumber.Length >= 6;
        }

        private static bool CheckName(string name, string surname)
        {
            return !name.Any(char.IsDigit) && !surname.Any(char.IsDigit);
        }
    }
}
