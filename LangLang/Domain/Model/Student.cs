using System;
using System.Collections.Generic;

namespace LangLang.Domain.Model
{
    public class Student : Person
    {
        public string Id { get; set; }
        
        public EducationLvl Education { get; set; }
        public uint PenaltyPts { get; set; }
        
        public Dictionary<string, LanguageLevel> CompletedCourseLanguages { get; set; }
        public Dictionary<string, LanguageLevel> PassedExamLanguages { get; set; }

        public Student() : base("", "", DateTime.Now, Gender.Other, "")
        {
            Id = "";
            PenaltyPts = 0;
            CompletedCourseLanguages = new Dictionary<string, LanguageLevel>();
            PassedExamLanguages = new Dictionary<string, LanguageLevel>();
        }

        public Student(string name, string surname, DateTime birthDate, Gender gender, string phoneNumber, EducationLvl educationLvl, uint penaltyPts, Dictionary<string, LanguageLevel>? completedCourseLanguages = null, Dictionary<string, LanguageLevel>? passedExamLanguages = null)
            : base(name, surname, birthDate, gender, phoneNumber)
        {
            Id = "";
            PenaltyPts = penaltyPts;
            Education = educationLvl;
            CompletedCourseLanguages = completedCourseLanguages ?? new Dictionary<string, LanguageLevel>();
            PassedExamLanguages = passedExamLanguages ?? new Dictionary<string, LanguageLevel>();
        }
        
        public Student(string id, string name, string surname, DateTime birthDate, Gender gender, string phoneNumber, EducationLvl educationLvl, uint penaltyPts, Dictionary<string, LanguageLevel> completedCourseLanguages, Dictionary<string, LanguageLevel> passedExamLanguages)
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

        public void AddCompletedCourseLanguage(Language language, LanguageLevel languageLevel)
        {
            CompletedCourseLanguages[language.Name] = languageLevel;
        }
        
        public void AddPassedExamLanguage(Language language, LanguageLevel languageLevel)
        {
            CompletedCourseLanguages[language.Name] = languageLevel;
        }
    }
    
    public enum EducationLvl
    {
        ElementarySchool, HighSchool, CollegeDegree, MastersDegree, PhD
    }
}