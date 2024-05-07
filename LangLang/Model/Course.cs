using Consts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Xml.Linq;

namespace LangLang.Model
{
    public class Course
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Language Language { get; set; }
        public LanguageLvl Level { get; set; }
        public int Duration { get; set; }
        public Dictionary<WorkDay,Tuple<TimeOnly,int>> Schedule { get; set; }
        public DateTime Start { get; set; }
        public bool Online { get; set; }
        public int MaxStudents { get; set; }
        public int NumStudents { get; set; }
        public enum CourseState
        {
            NotStarted, Locked, Canceled, InProgress, FinishedNotGraded, FinishedGraded
        }

        public CourseState State { get; set; }

        
        public Course()
        {
            Id = "0";
            Name = "";
            Language = new Language("English", "en");
            Level = LanguageLvl.A1;
            Duration = 0;
            Schedule = new Dictionary<WorkDay, Tuple<TimeOnly, int>>();
            Start = DateTime.Now;
            Online = false;
            MaxStudents = 0;
            NumStudents = 0;
            State = CourseState.Canceled;
        }
        public Course(string id,string name, Language language, LanguageLvl level, int duration, Dictionary<WorkDay, Tuple<TimeOnly, int>> schedule, DateTime start, bool online, int numStudents, CourseState state, int maxStudents = 0)
        {
            Id = id;
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
        }
        // Constructor without id when creating a new course
        public Course(string name, Language language, LanguageLvl level, int duration, Dictionary<WorkDay, Tuple<TimeOnly, int>> schedule, DateTime start, bool online, int numStudents, CourseState state, int maxStudents = 0)
        { 
            Id = "0";
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

        public bool CanBeModified()
        {
            if (State == CourseState.NotStarted)
            {
                return true;
            }
            return false;
        }

    }
}
