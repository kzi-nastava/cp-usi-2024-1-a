using System;
using System.Collections.Generic;
using LangLang.Domain.Model;

namespace LangLang.Application.DTO;

public class RegisterTutorDto : RegisterDto
{
    public List<Tuple<Language, LanguageLevel>> KnownLanguages { get; }
    public DateTime DateAdded { get; }
    
    public RegisterTutorDto(string email, string password, string name, string surname, DateTime birthDay, Gender gender, string phoneNumber, List<Tuple<Language, LanguageLevel>> knownLanguages, DateTime dateAdded)
        : base(email, password, name, surname, birthDay, gender, phoneNumber)
    {
        KnownLanguages = knownLanguages;
        DateAdded = dateAdded;
    }
}