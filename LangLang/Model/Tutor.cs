using Consts;
using System;
using System.Collections.Generic;

public class Tutor : User
{
    public List<Language> KnownLanguages {get; set;}
    public List<Course> Courses { get; set; }
    public double ScoreAvg { get; set; }
    // TODO: create exam model and add it
    //public List<Exam> Exams;
    
    
    public Tutor(string email, string password, string name, string surname, DateTime birthDate, Gender gender, string phoneNumber, List<Language> knownLanguages, List<Course> courses, double scoreAvg) : base(email, password, name, surname, birthDate, gender, phoneNumber)
    {
        ScoreAvg = scoreAvg;
        KnownLanguages = knownLanguages;
        Courses = courses;

    }

}
