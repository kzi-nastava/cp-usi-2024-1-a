using Consts;
using System;

namespace LangLang.Model
{
    public class Student : User
    {
        public EducationLvl Education { get; set; }
        public uint PenaltyPts { get; set; }

        public Student() : base("", "", "", "", DateTime.Now, Gender.Other, "")
        {
            PenaltyPts = 0;
        }

        public Student(string email, string password, string name, string surname, DateTime birthDate, Gender gender, string phoneNumber, EducationLvl educationLvl, uint penaltyPts)
            : base(email, password, name, surname, birthDate, gender, phoneNumber)
        {
            PenaltyPts = penaltyPts;
            Education = educationLvl;
        }

        public void RemovePenaltyPts()
        {
            if (PenaltyPts > 0) { PenaltyPts--; }
        }
    }
}

