using System;
using Consts;

namespace LangLang.Model;

public abstract class Person
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string PhoneNumber { get; set; }

    protected Person(string name, string surname, DateTime birthDate, Gender gender, string phoneNumber)
    {
        Name = name;
        Surname = surname;
        BirthDate = birthDate;
        Gender = gender;
        PhoneNumber = phoneNumber;
    }
}