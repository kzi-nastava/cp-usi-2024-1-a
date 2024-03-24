using Consts;
using System;
using System.Collections.Generic;

public class Tutor : User
{
    private List<Language> knownLanguages;
    private List<Course> courses;
    private double scoreAvg;
    // TODO: create exam model and add get/set and attribute in constructor
    //public List<Exam> exams;

    public List<Language> KnownLanguages {get; set;}
    public List<Course> Courses { get; set; }
    public double ScoreAvg { get; set; }
    
    
    public Tutor(string email, string password, string name, string surname, DateTime birthDate, Gender gender, string phoneNumber, List<Language> knownLanguages, List<Course> courses, double scoreAvg) : base(email, password, name, surname, birthDate, gender, phoneNumber)
    {
        ScoreAvg = scoreAvg;
        KnownLanguages = knownLanguages;
        Courses = courses;

    }

}
