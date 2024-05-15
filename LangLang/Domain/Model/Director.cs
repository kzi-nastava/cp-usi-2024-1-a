using System;

namespace LangLang.Domain.Model
{
    public class Director : Person, IEntity
    {
        public string Id { get; set; }
        
        public Director() : base("", "", DateTime.Now, Gender.Other, "")
        {
            Id = "";
        }

        public Director(string name, string surname, DateTime birthDate, Gender gender, string phoneNumber) : base(name, surname, birthDate, gender, phoneNumber)
        {
            Id = "";
        }
        
        public Director(string id, string name, string surname, DateTime birthDate, Gender gender, string phoneNumber) : base(name, surname, birthDate, gender, phoneNumber)
        {
            Id = id;
        }
    }
}
