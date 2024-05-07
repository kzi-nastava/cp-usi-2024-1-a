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
        
        public Dictionary<string, LanguageLvl> CompletedCourseLanguages { get; set; }
        public Dictionary<string, LanguageLvl> PassedExamLanguages { get; set; }

        public Student() : base("", "", DateTime.Now, Gender.Other, "")
        {
            Id = "";
            PenaltyPts = 0;
            CompletedCourseLanguages = new Dictionary<string, LanguageLvl>();
            PassedExamLanguages = new Dictionary<string, LanguageLvl>();
        }

        public Student(string name, string surname, DateTime birthDate, Gender gender, string phoneNumber, EducationLvl educationLvl, uint penaltyPts, Dictionary<string, LanguageLvl>? completedCourseLanguages = null, Dictionary<string, LanguageLvl>? passedExamLanguages = null)
            : base(name, surname, birthDate, gender, phoneNumber)
        {
            Id = "";
            PenaltyPts = penaltyPts;
            Education = educationLvl;
            CompletedCourseLanguages = completedCourseLanguages ?? new Dictionary<string, LanguageLvl>();
            PassedExamLanguages = passedExamLanguages ?? new Dictionary<string, LanguageLvl>();
        }
        
        public Student(string id, string name, string surname, DateTime birthDate, Gender gender, string phoneNumber, EducationLvl educationLvl, uint penaltyPts, Dictionary<string, LanguageLvl> completedCourseLanguages, Dictionary<string, LanguageLvl> passedExamLanguages)
            : base(name, surname, birthDate, gender, phoneNumber)
        {
            Id = id;
            PenaltyPts = penaltyPts;
            CompletedCourseLanguages = completedCourseLanguages;
            PassedExamLanguages = passedExamLanguages;
            Education = educationLvl;
        }

        public void RemovePenaltyPts()
        {
            if (PenaltyPts > 0) { PenaltyPts--; }
        }

        public void AddCompletedCourseLanguage(Language language, LanguageLvl languageLvl)
        {
            CompletedCourseLanguages[language.Name] = languageLvl;
        }
        
        public void AddPassedExamLanguage(Language language, LanguageLvl languageLvl)
        {
            CompletedCourseLanguages[language.Name] = languageLvl;
        }
    }
}