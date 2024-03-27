using Consts;
using System;
using System.Collections.Generic;

namespace LangLang.Model
{
    public class Tutor : User
    {
        public List<Tuple<Language, LanguageLvl>> KnownLanguages { get; set; }
        public List<Course> Courses { get; set; }
        public List<Exam> Exams { get; set; }
        public double ScoreAvg { get; set; }


        public Tutor(string email, string password, string name, string surname, DateTime birthDate, Gender gender, string phoneNumber, List<Tuple<Language, LanguageLvl>> knownLanguages, List<Course> courses, List<Exam> exams, double scoreAvg) : base(email, password, name, surname, birthDate, gender, phoneNumber)
        {
            ScoreAvg = scoreAvg;
            KnownLanguages = knownLanguages;
            Courses = courses;
            Exams = exams;
        }
    }
}