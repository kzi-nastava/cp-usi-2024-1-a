using System;
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

        public State ExamState { get; set; }
        
        [JsonIgnore]
        public DateOnly Date => DateOnly.FromDateTime(Time.Date);
        [JsonIgnore]
        public TimeOnly TimeOfDay => TimeOnly.FromDateTime(Time);
        
        public Exam()
        {
            Id = "";
            Language = new Language();
            LanguageLvl = LanguageLvl.A1;
        }
        
        public Exam(Language language, LanguageLvl languageLvl, DateTime time, int classroomNumber, State examState, int maxStudents, int numStudents=0)
        {
            Id = "";
            Language = language;
            LanguageLvl = languageLvl;
            Time = time;
            ClassroomNumber = classroomNumber;
            MaxStudents = maxStudents;
            NumStudents = numStudents;
            ExamState = examState;
        }
        
        public Exam(string id, Language language, LanguageLvl languageLvl, DateTime time, int classroomNumber, State examState, int maxStudents, int numStudents=0)
        {
            Id = id;
            Language = language;
            LanguageLvl = languageLvl;
            Time = time;
            ClassroomNumber = classroomNumber;
            MaxStudents = maxStudents;
            NumStudents = numStudents;
            ExamState = examState;
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
        
        public enum State
        {
            /// <summary>
            /// Created, not started, without modification limitations.
            /// </summary>
            NotStarted,

            /// <summary>
            /// Will not be realized.
            /// </summary>
            Canceled,

            /// <summary>
            /// Cannot be modified, but students can apply and/or be accepted/rejected.
            /// </summary>
            Locked,

            /// <summary>
            /// Cannot be modified, students cannot apply/be accepted/be rejected.
            /// </summary>
            Confirmed,

            /// <summary>
            /// Students can be kicked out.
            /// </summary>
            InProgress,

            /// <summary>
            /// Finished, not yet graded.
            /// </summary>
            Finished,

            /// <summary>
            /// Finished, graded, reports not yet sent to students.
            /// </summary>
            Graded,

            /// <summary>
            /// Finished, graded, grade reports sent, nothing can be modified.
            /// </summary>
            Reported
        }
    }
}