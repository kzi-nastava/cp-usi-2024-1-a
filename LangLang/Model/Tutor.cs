using Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace LangLang.Model
{
    public class Tutor : Person
    {
        public string Id { get; set; }
        
        /// <summary> Hold counts for 1 to 10 rating. </summary>
        public int[] RatingCounts { get; set; } = new int[10];

        public List<Tuple<Language, LanguageLvl>> KnownLanguages { get; set; }
        public List<string> Courses { get; set; }
        public List<string> Exams { get; set; }
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

        public Tutor() : base("", "", DateTime.Now, Gender.Other, "")
        {
            Id = "";
            KnownLanguages = new();
            Courses = new();
            Exams = new();
            DateAdded = DateTime.Now;
        }
		
        public Tutor(string name, string surname, DateTime birthDate, Gender gender, string phoneNumber, List<Tuple<Language, LanguageLvl>> knownLanguages, List<string> courses, List<string> exams, int[] ratingCounts, DateTime? dateAdded = null) : base(name, surname, birthDate, gender, phoneNumber)
        {
            Id = "";
            KnownLanguages = knownLanguages;
            Courses = courses;
            Exams = exams;
            this.RatingCounts = ratingCounts;
            if (dateAdded == null)
                DateAdded = DateTime.Now;
            else
                DateAdded = (DateTime)dateAdded;
        }
        public Tutor(string id, string name, string surname, DateTime birthDate, Gender gender, string phoneNumber, List<Tuple<Language, LanguageLvl>> knownLanguages, List<string> courses, List<string> exams, int[] ratingCounts, DateTime? dateAdded = null) : base(name, surname, birthDate, gender, phoneNumber)
        {
            Id = id;
            KnownLanguages = knownLanguages;
            Courses = courses;
            Exams = exams;
            this.RatingCounts = ratingCounts;
            if (dateAdded == null)
                DateAdded = DateTime.Now;
            else
                DateAdded = (DateTime)dateAdded;
        }

        public double GetAverageRating()
        {
            int sum = 0;
            for (int i = 0; i < RatingCounts.Length; i++)
            {
                sum += (i+1) * RatingCounts[i];
            }
            return (double)sum / RatingCounts.Length;
        }

        public void AddRating(int rating)
        {
            rating--;
            if (rating < 0 || rating > RatingCounts.Length)
                throw new ArgumentOutOfRangeException($"Rating is too " + ((rating < 0) ? "low" : "high"));
            else
                RatingCounts[rating]++;
        }
    }
}