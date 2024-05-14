using System;
using LangLang.Application.DTO;
using LangLang.Application.UseCases.User;
using LangLang.Application.Utility.Validators;
using LangLang.Domain.Model;

namespace LangLang.Application.UseCases.Authentication
{
    public class RegisterService : IRegisterService
    {
        private readonly IUserValidator _userValidator;
        private readonly IAccountService _accountService;

        public RegisterService(IUserValidator userValidator, IAccountService accountService)
        {
            _userValidator = userValidator;
            _accountService = accountService;
        }

        public ValidationError RegisterStudent(string? email, string? password, string? name, string? surname, DateTime birthDay, Gender gender, string? phoneNumber, EducationLevel educationLevel)
        {
            ValidationError error = ValidationError.None;
            error |= _userValidator.CheckUserData(email, password, name, surname, phoneNumber, birthDay);
            error |= _userValidator.EmailTaken(email!);

            if (error == ValidationError.None)
            {
                _accountService.RegisterStudent(new RegisterStudentDto(
                    email!,
                    password!,
                    name!,
                    surname!,
                    birthDay,
                    gender,
                    phoneNumber!,
                    educationLevel
                    ));
            }
            return error;
        }
    }
}
