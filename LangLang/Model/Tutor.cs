using Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace LangLang.Model
{
    public class Tutor : User
    {
        /// <summary> Hold counts for 1 to 5 rating. </summary>
        private int[] scoreCounts = new int[5];

        public List<Tuple<Language, LanguageLvl>> KnownLanguages { get; set; }
        public List<Course> Courses { get; set; }
        public List<Exam> Exams { get; set; }

        public Tutor(string email, string password, string name, string surname, DateTime birthDate, Gender gender, string phoneNumber, List<Tuple<Language, LanguageLvl>> knownLanguages, List<Course> courses, List<Exam> exams, int[] scoreCounts) : base(email, password, name, surname, birthDate, gender, phoneNumber)
        {
            KnownLanguages = knownLanguages;
            Courses = courses;
            Exams = exams;
            this.scoreCounts = scoreCounts;
        }

        public double GetAverageScore()
        {
            int sum = 0;
            for (int i = 0; i < scoreCounts.Length; i++)
            {
                sum += (i+1) * scoreCounts[i];
            }
            return (double)sum / scoreCounts.Length;
        }
    }
}