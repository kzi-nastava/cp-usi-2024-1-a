using System;
using System.Text.Json.Serialization;

namespace LangLang.Domain.Model
{
    public class Exam : IEntity
    {
        public string Id { get; set; }
        public Language Language { get; set; }
        public LanguageLevel LanguageLevel { get; set; }
        public DateTime Time { get; set; }
        public int ClassroomNumber { get; set; }
        public int MaxStudents { get; set; }
        public int NumStudents { get; private set; }

        public State ExamState { get; private set; }

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

        public string? TutorId { get; set; }

        [JsonIgnore] public DateOnly Date => DateOnly.FromDateTime(Time.Date);
        [JsonIgnore] public TimeOnly TimeOfDay => TimeOnly.FromDateTime(Time);

        public Exam()
        {
            Id = "";
            Language = new Language();
            LanguageLevel = LanguageLevel.A1;
        }

        public Exam(string tutorId, Language language, LanguageLevel languageLevel, DateTime time, int classroomNumber, State examState,
            int maxStudents, int numStudents = 0)
        {
            Id = "";
            TutorId = tutorId;
            Language = language;
            LanguageLevel = languageLevel;
            Time = time;
            ClassroomNumber = classroomNumber;
            MaxStudents = maxStudents;
            NumStudents = numStudents;
            ExamState = examState;
            UpdateExamStateBasedOnCurrentDateTime();
        }

        public Exam(string id, string tutorId, Language language, LanguageLevel languageLevel, DateTime time, int classroomNumber,
            State examState, int maxStudents, int numStudents = 0)
        {
            Id = id;
            TutorId = tutorId;
            Language = language;
            LanguageLevel = languageLevel;
            Time = time;
            ClassroomNumber = classroomNumber;
            MaxStudents = maxStudents;
            NumStudents = numStudents;
            ExamState = examState;
            UpdateExamStateBasedOnCurrentDateTime();
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

        public bool MatchesFilter(Language? language = null, LanguageLevel? languageLvl = null,
            DateOnly? date = null)
        {
            if (language != null && !Equals(Language, language))
                return false;
            if (languageLvl != null && LanguageLevel != languageLvl)
                return false;
            if (date != null && Date != date)
                return false;
            return true;
        }

        private static State GetExamStateBasedOnDateTime(DateTime dateTime, DateOnly examDate,
            TimeOnly examTime)
        {
            var examDateTime = examDate.ToDateTime(examTime);
            if (dateTime < examDateTime.Subtract(Constants.LockedExamTime))
                return State.NotStarted;
            if (dateTime < examDateTime.Subtract(Constants.ConfirmedExamTime))
                return State.Locked;
            if (dateTime < examDateTime)
                return State.Confirmed;
            if (dateTime < examDateTime.Add(Constants.ExamDuration))
                return State.InProgress;
            return State.Finished;
        }

        public void UpdateExamStateBasedOnCurrentDateTime()
        {
            if (ExamState is State.Canceled or State.Graded or State.Reported)
                return;
            ExamState = GetExamStateBasedOnDateTime(DateTime.Now, Date, TimeOfDay);
        }

        public void Finish()
        {
            if (ExamState != State.InProgress)
                throw new InvalidOperationException("Cannot finish exam with current state.");
            ExamState = State.Finished;
        }

        public void Confirm()
        {
            if (ExamState != State.NotStarted)
                throw new InvalidOperationException("Cannot confirm exam with current state.");
            ExamState = State.Confirmed;
        }

        public bool IsAvailable(Student student)
        {
            if (ExamState != State.NotStarted)
                return false;
            if (IsFull())
                return false;
            if (!student.HasCourseKnowledge(Language, LanguageLevel))
                return false;
            if (student.HasExamKnowledge(Language, LanguageLevel))
                return false;
            return true;
        }

        public bool CanBeUpdated()
        {
            return ExamState == State.NotStarted;
        }

        public void Update(Language language, LanguageLevel languageLevel, DateTime dateTime, int classroomNumber,
            int maxStudents)
        {
            if (!CanBeUpdated())
                throw new ArgumentException("Exam cannot be updated at this state");
            Language = language;
            LanguageLevel = languageLevel;
            Time = dateTime;
            ClassroomNumber = classroomNumber;
            if (NumStudents > maxStudents)
                throw new ArgumentException("Maximum number of students cannot exceed the current number of students.");
            MaxStudents = maxStudents;
            UpdateExamStateBasedOnCurrentDateTime();
        }
    }
}