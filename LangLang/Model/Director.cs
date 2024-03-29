using Consts;
using System;

namespace LangLang.Model
{
    public class Director : User
    {
        public Director() : base("", "", "", "", DateTime.Now, Gender.Other, "")
        {
        }

        public Director(string email, string password, string name, string surname, DateTime birthDate, Gender gender, string phoneNumber) : base(email, password, name, surname, birthDate, gender, phoneNumber)
        {
        }
    }
}
