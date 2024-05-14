using System;
using LangLang.Domain.Model;

namespace LangLang.Application.UseCases.Authentication;

public interface IRegisterService
{
    public ValidationError RegisterStudent(string? email, string? password, string? name, string? surname, DateTime birthDay,
        Gender gender, string? phoneNumber, EducationLevel educationLevel);
}