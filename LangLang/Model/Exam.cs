using System;
using System.Security.Cryptography;
using System.Text.Json.Serialization;
using Consts;

namespace LangLang.Model
{
    public class Exam
    {
        public string Id { get; set; }
        public Language Language { get; set; }
        public LanguageLvl LanguageLvl { get; set; }
        public DateTime Time { get; set; }
        public int ClassroomNumber { get; set; }
        public int MaxStudents { get; set; }
        public int NumStudents { get; set; }

        [JsonIgnore]
        public DateOnly Date => DateOnly.FromDateTime(Time.Date);
        [JsonIgnore]
        public TimeSpan TimeOfDay => Time.TimeOfDay;
        
        public Exam()
        {
            Id = "";
            Language = new Language();
            LanguageLvl = LanguageLvl.A1;
        }
        
        public Exam(Language language, LanguageLvl languageLvl, DateTime time, int classroomNumber, int maxStudents, int numStudents=0)
        {
            Id = "";
            Language = language;
            LanguageLvl = languageLvl;
            Time = time;
            ClassroomNumber = classroomNumber;
            MaxStudents = maxStudents;
            NumStudents = numStudents;
        }
        
        public Exam(string id, Language language, LanguageLvl languageLvl, DateTime time, int classroomNumber, int maxStudents, int numStudents=0)
        {
            Id = id;
            Language = language;
            LanguageLvl = languageLvl;
            Time = time;
            ClassroomNumber = classroomNumber;
            MaxStudents = maxStudents;
            NumStudents = numStudents;
        }
        
        public void AddAttendance()
        {
            NumStudents++;
        }

        public void CancelAttendance()
        {
            NumStudents--;
        }

        public bool IsFull()
        {
            return NumStudents == MaxStudents;
        }
    }
}