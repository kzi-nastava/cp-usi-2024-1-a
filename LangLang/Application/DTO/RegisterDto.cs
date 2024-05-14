using System;
using LangLang.Domain.Model;

namespace LangLang.Application.DTO;

public class RegisterDto
{
    public string Email { get; }
    public string Password { get; }
    public string Name { get; }
    public string Surname { get; }
    public DateTime BirthDay { get; }
    public Gender Gender { get; }
    public string PhoneNumber { get;}

    public RegisterDto(string email, string password, string name, string surname, DateTime birthDay, Gender gender, string phoneNumber)
    {
        Email = email;
        Password = password;
        Name = name;
        Surname = surname;
        BirthDay = birthDay;
        Gender = gender;
        PhoneNumber = phoneNumber;
    }
}