using System;
using System.Collections.Generic;

namespace LangLang.Domain.Model
{
    public class Tutor : Person, IEntity
    {
        [SkipInForm] public string Id { get; set; }
        
        /// <summary> Hold counts for 1 to 10 rating. </summary>
        [Skip] public int[] RatingCounts { get; set; } = new int[10];

        [Skip] public List<Tuple<Language, LanguageLevel>> KnownLanguages { get; set; }

        [SkipInForm] public DateTime DateAdded { get; set; }
        
        public Tutor() : base("", "", DateTime.Now, Gender.Other, "")
        {
            Id = "";
            KnownLanguages = new();
            DateAdded = DateTime.Now;
        }
		
        public Tutor(string name, string surname, DateTime birthDate, Gender gender, string phoneNumber, List<Tuple<Language, LanguageLevel>> knownLanguages, int[] ratingCounts, DateTime? dateAdded = null) : base(name, surname, birthDate, gender, phoneNumber)
        {
            Id = "";
            KnownLanguages = knownLanguages;
            this.RatingCounts = ratingCounts;
            if (dateAdded == null)
                DateAdded = DateTime.Now;
            else
                DateAdded = (DateTime)dateAdded;
        }
        public Tutor(string id, string name, string surname, DateTime birthDate, Gender gender, string phoneNumber, List<Tuple<Language, LanguageLevel>> knownLanguages, int[] ratingCounts, DateTime? dateAdded = null) : base(name, surname, birthDate, gender, phoneNumber)
        {
            Id = id;
            KnownLanguages = knownLanguages;
            this.RatingCounts = ratingCounts;
            if (dateAdded == null)
                DateAdded = DateTime.Now;
            else
                DateAdded = (DateTime)dateAdded;
        }

        public string GetFullName()
        {
            return Name + " " + Surname;
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

        public bool KnowsLanguage(Language language, LanguageLevel languageLevel)
        {
            foreach (var knownLanguage in KnownLanguages)
            {
                if(!Equals(knownLanguage.Item1, language))
                    continue;
                if (knownLanguage.Item2 < languageLevel)
                    return false;
                return true;
            }
            return false;
        }

        public double GetScore(Language language, LanguageLevel languageLevel)
        {
            var knownLanguageLevelIdx = -1;
            for (int i = 0; i < KnownLanguages.Count; i++)
            {
                if (Equals(KnownLanguages[i].Item1, language))
                {
                    knownLanguageLevelIdx = i;
                    break;
                }
            }

            int languageLevelScore = knownLanguageLevelIdx == -1
                ? -(int)languageLevel
                : KnownLanguages[knownLanguageLevelIdx].Item2 - languageLevel;
            
            return GetAverageRating() == 0 ? 1000 : 0
                + 10 * languageLevelScore
                + 100 * GetAverageRating()
                ;
        }

        public bool MatchesFilter(string? languageName = null, LanguageLevel? languageLevel = null, 
                                  DateTime? dateAddedMin = null, DateTime? dateAddedMax = null)
        {
            if (languageName != ""
              && !KnownLanguages.Exists(tuple => tuple.Item1.Name == languageName))
                return false;
            if (languageLevel != null
              && !KnownLanguages.Exists(tuple => tuple.Item2 == languageLevel))
                return false;
            if (dateAddedMin != null && DateAdded < dateAddedMin)
                return false;
            if (dateAddedMax != null && DateAdded > dateAddedMax)
                return false;
            return true;
        }
    }
}