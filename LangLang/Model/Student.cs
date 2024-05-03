using Consts;
using System;
using System.Collections.Generic;

namespace LangLang.Model
{
    public class Student : Person
    {
        public string Id { get; set; }
        
        public EducationLvl Education { get; set; }
        public uint PenaltyPts { get; set; }
        
        public Dictionary<Language, LanguageLvl> KnownLanguages { get; }

        public Student() : base("", "", DateTime.Now, Gender.Other, "")
        {
            Id = "";
            PenaltyPts = 0;
            KnownLanguages = new Dictionary<Language, LanguageLvl>();
        }

        public Student(string name, string surname, DateTime birthDate, Gender gender, string phoneNumber, EducationLvl educationLvl, uint penaltyPts)
            : base(name, surname, birthDate, gender, phoneNumber)
        {
            Id = "";
            PenaltyPts = penaltyPts;
            Education = educationLvl;
            KnownLanguages = new Dictionary<Language, LanguageLvl>();
        }
        
        public Student(string id, string name, string surname, DateTime birthDate, Gender gender, string phoneNumber, EducationLvl educationLvl, uint penaltyPts, Dictionary<Language, LanguageLvl> knownLanguages)
            : base(name, surname, birthDate, gender, phoneNumber)
        {
            Id = id;
            PenaltyPts = penaltyPts;
            KnownLanguages = knownLanguages;
            Education = educationLvl;
        }

        public void RemovePenaltyPts()
        {
            if (PenaltyPts > 0) { PenaltyPts--; }
        }

        public void AddKnownLanguage(Language language, LanguageLvl languageLvl)
        {
            KnownLanguages[language] = languageLvl;
        }
    }
}