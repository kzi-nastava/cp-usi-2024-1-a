using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace LangLang.Domain.Model
{
    public class Course : IEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Language Language { get; set; }
        public LanguageLevel Level { get; set; }
        public int Duration { get; set; }

        [NotMapped]
        public Dictionary<WorkDay,Tuple<TimeOnly,int>> Schedule { get; set; }
        public string ScheduleSerialized
        {
            get => JsonSerializer.Serialize(Schedule);
            set => Schedule = JsonSerializer.Deserialize<Dictionary<WorkDay, Tuple<TimeOnly, int>>>(value)!;
        }
        public DateTime Start { get; set; }
        public bool Online { get; set; }
        public int MaxStudents { get; set; }
        public int NumStudents { get; set; }
        public enum CourseState
        {
            NotStarted, Locked, Canceled, InProgress, FinishedNotGraded, FinishedGraded
        }

        public CourseState State { get; set; }

        public string? TutorId { get; set; }
        
        public bool IsCreatedByTutor { get; set; }
        
        public Course()
        {
            Id = "-1";
            Name = "";
            Language = new Language("English", "en");
            Level = LanguageLevel.A1;
            Duration = 0;
            Schedule = new Dictionary<WorkDay, Tuple<TimeOnly, int>>();
            Start = DateTime.Now;
            Online = false;
            MaxStudents = 0;
            NumStudents = 0;
            State = CourseState.Canceled;
            IsCreatedByTutor = true;
        }
        public Course(string id, string tutorId, string name, Language language, LanguageLevel level, int duration, Dictionary<WorkDay, Tuple<TimeOnly, int>> schedule, DateTime start, bool online, int numStudents, CourseState state, int maxStudents = 0, bool isCreatedByTutor = true)
        {
            Id = id;
            TutorId = tutorId;
            Name = name;
            Language = language;
            Level = level;
            Duration = duration;
            Schedule = schedule;
            Start = start;
            Online = online;
            MaxStudents = maxStudents;
            NumStudents = numStudents;
            State = state;
            IsCreatedByTutor = isCreatedByTutor;
        }
        // Constructor without id when creating a new course
        public Course(string tutorId, string name, Language language, LanguageLevel level, int duration, Dictionary<WorkDay, Tuple<TimeOnly, int>> schedule, DateTime start, bool online, int numStudents, CourseState state, int maxStudents = 0, bool isCreatedByTutor = true)
        { 
            Id = "-1";
            TutorId = tutorId;
            Name = name;
            Language = language;
            Level = level;
            Duration = duration;
            Schedule = schedule;
            Start = start;
            Online = online;
            MaxStudents = maxStudents;
            NumStudents = numStudents;
            State = state;
            IsCreatedByTutor = isCreatedByTutor;
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

        public bool IsActive()
        {
            return State != CourseState.NotStarted && State != CourseState.FinishedGraded;
        }

        public bool IsFinished()
        {
            return State == CourseState.FinishedGraded || State == CourseState.FinishedNotGraded;
        }

        public bool IsFinishedAndGraded()
        {
            return State == CourseState.FinishedGraded;
        }


        public bool CanBeModified()
        {
            return State == CourseState.NotStarted;
        }

        public bool IsApplicable()
        {
            return State == CourseState.NotStarted && !IsFull();
        }

        public void Finish()
        {
            State = CourseState.FinishedGraded;
        }


        public void UpdateState() {
            if (Start <= DateTime.Now && Start + Duration * Constants.CancellableCourseTime >= DateTime.Now)
            {
                State = CourseState.InProgress;
            }
            else if (Start - Constants.CancellableCourseTime < DateTime.Now && Start >= DateTime.Now)
            {
                State = CourseState.Locked;
            }
            else if (Start - Constants.CancellableCourseTime >= DateTime.Now)
            {
                State = CourseState.NotStarted;
            }
            else
            {
                if(State != CourseState.FinishedGraded)
                {
                    State = CourseState.FinishedNotGraded;
                }
            }
        }

        public static bool IsValid(string name, string? languageName, LanguageLevel? level, int? duration, CourseState? state, DateTime? start, int maxStudents, ObservableCollection<WorkDay> scheduleDays)
        {
            return !(name == "" || languageName == null || duration == null || start == null || maxStudents == 0 || level == null || state == null || scheduleDays.Count == 0);
        }

        public bool MatchesFilter(string? language, LanguageLevel? level, DateTime? start, bool? online, int? duration)
        {
            if (language != null && language != Language.Name) return false;

            if (level != null && level != Level) return false;

            if (start != null && !Equals(start, Start)) return false;

            if (online != null && online != Online) return false;

            if (duration != null && duration != Duration) return false;

            return true;
        }

    }
}
