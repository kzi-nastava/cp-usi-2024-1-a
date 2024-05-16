using System;
using System.Collections.Generic;

namespace LangLang.Domain.Model
{
    public class Student : Person, IEntity
    {
        public string Id { get; set; }
        
        public EducationLevel Education { get; set; }
        public uint PenaltyPoints { get; set; }
        
        public Dictionary<string, LanguageLevel> CompletedCourseLanguages { get; set; }
        public Dictionary<string, LanguageLevel> PassedExamLanguages { get; set; }

        public Student() : base("", "", DateTime.Now, Gender.Other, "")
        {
            Id = "";
            PenaltyPoints = 0;
            CompletedCourseLanguages = new Dictionary<string, LanguageLevel>();
            PassedExamLanguages = new Dictionary<string, LanguageLevel>();
        }

        public Student(string name, string surname, DateTime birthDate, Gender gender, string phoneNumber, EducationLevel educationLevel, uint penaltyPoints, Dictionary<string, LanguageLevel>? completedCourseLanguages = null, Dictionary<string, LanguageLevel>? passedExamLanguages = null)
            : base(name, surname, birthDate, gender, phoneNumber)
        {
            Id = "";
            PenaltyPoints = penaltyPoints;
            Education = educationLevel;
            CompletedCourseLanguages = completedCourseLanguages ?? new Dictionary<string, LanguageLevel>();
            PassedExamLanguages = passedExamLanguages ?? new Dictionary<string, LanguageLevel>();
        }
        
        public Student(string id, string name, string surname, DateTime birthDate, Gender gender, string phoneNumber, EducationLevel educationLvl, uint penaltyPoints, Dictionary<string, LanguageLevel> completedCourseLanguages, Dictionary<string, LanguageLevel> passedExamLanguages)
            : base(name, surname, birthDate, gender, phoneNumber)
        {
            Id = id;
            PenaltyPoints = penaltyPoints;
            CompletedCourseLanguages = completedCourseLanguages;
            PassedExamLanguages = passedExamLanguages;
            Education = educationLvl;
        }

        public void Update(string name, string surname, DateTime birthDate, Gender gender, string phoneNumber)
        {
            Name = name;
            Surname = surname;
            Gender = gender;
            BirthDate = birthDate;
            Gender = gender;
            PhoneNumber = phoneNumber;
        }

        public void AddPenaltyPoint()
        {
            PenaltyPoints++; 
        }

        public void RemovePenaltyPoint()
        {
            if (PenaltyPoints > 0) { PenaltyPoints--; }
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
    
    public enum EducationLevel
    {
        ElementarySchool, HighSchool, CollegeDegree, MastersDegree, PhD
    }
}