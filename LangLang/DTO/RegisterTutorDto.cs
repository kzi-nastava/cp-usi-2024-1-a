using System;
using System.Collections.Generic;
using Consts;
using LangLang.Model;

namespace LangLang.DTO;

public class RegisterTutorDto : RegisterDto
{
    public List<Tuple<Language, LanguageLvl>> KnownLanguages { get; }
    public DateTime DateAdded { get; }
    
    public RegisterTutorDto(string email, string password, string name, string surname, DateTime birthDay, Gender gender, string phoneNumber, List<Tuple<Language, LanguageLvl>> knownLanguages, DateTime dateAdded)
        : base(email, password, name, surname, birthDay, gender, phoneNumber)
    {
        KnownLanguages = knownLanguages;
        DateAdded = dateAdded;
    }
}