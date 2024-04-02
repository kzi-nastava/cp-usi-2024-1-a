using Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace LangLang.Model
{
    public class Tutor : User
    {
        /// <summary> Hold counts for 1 to 5 rating. </summary>
        private int[] ratingCounts = new int[5];

        public List<Tuple<Language, LanguageLvl>> KnownLanguages { get; set; }
        public List<Course> Courses { get; set; }
        public List<Exam> Exams { get; set; }
        public DateTime DateAdded { get; set; }
        
        public string KnownLanguagesAsString
        {
            get
            {
                StringBuilder builder = new();
                foreach (var tuple in KnownLanguages)
                    builder.AppendLine($"{tuple.Item1.Name} - {tuple.Item2.ToStr()}");
                return builder.ToString().TrimEnd();
            }
        }

        public Tutor() : base("", "", "", "", DateTime.Now, Gender.Other, "")
        {
            KnownLanguages = new();
            Courses = new();
            Exams = new();
            DateAdded = DateTime.Now;
        }
        public Tutor(string email, string password, string name, string surname, DateTime birthDate, Gender gender, string phoneNumber, List<Tuple<Language, LanguageLvl>> knownLanguages, List<Course> courses, List<Exam> exams, int[] ratingCounts, DateTime? dateAdded = null) : base(email, password, name, surname, birthDate, gender, phoneNumber)
        {
            KnownLanguages = knownLanguages;
            Courses = courses;
            Exams = exams;
            this.ratingCounts = ratingCounts;
            if (dateAdded == null)
                DateAdded = DateTime.Now;
            else
                DateAdded = (DateTime)dateAdded;
        }

        public double GetAverageRating()
        {
            int sum = 0;
            for (int i = 0; i < ratingCounts.Length; i++)
            {
                sum += (i+1) * ratingCounts[i];
            }
            return (double)sum / ratingCounts.Length;
        }

        public void AddRating(int rating)
        {
            rating--;
            if (rating < 0 || rating > ratingCounts.Length)
                throw new ArgumentOutOfRangeException($"Rating is too " + ((rating < 0) ? "low" : "high"));
            else
                ratingCounts[rating]++;
        }
    }
}