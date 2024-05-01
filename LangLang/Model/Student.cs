using Consts;
using System;

namespace LangLang.Model
{
    public class Student : Person
    {
        public string Id { get; set; }
        
        public EducationLvl Education { get; set; }
        public uint PenaltyPts { get; set; }

        public Student() : base("", "", DateTime.Now, Gender.Other, "")
        {
            Id = "";
            PenaltyPts = 0;
        }

        public Student(string name, string surname, DateTime birthDate, Gender gender, string phoneNumber, EducationLvl educationLvl, uint penaltyPts)
            : base(name, surname, birthDate, gender, phoneNumber)
        {
            Id = "";
            PenaltyPts = penaltyPts;
            Education = educationLvl;
        }
        
        public Student(string id, string name, string surname, DateTime birthDate, Gender gender, string phoneNumber, EducationLvl educationLvl, uint penaltyPts)
            : base(name, surname, birthDate, gender, phoneNumber)
        {
            Id = id;
            PenaltyPts = penaltyPts;
            Education = educationLvl;
        }

        public void RemovePenaltyPts()
        {
            if (PenaltyPts > 0) { PenaltyPts--; }
        }
    }
}