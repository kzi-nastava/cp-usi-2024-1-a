using Consts;
using System;
using System.Collections.Generic;

namespace LangLang.Model
{
    internal class Course
    {
        public string Name { get; set; }
        public Language Language { get; set; }
        public LanguageLvl Level { get; set; }
        public int Duration { get; set; }
        public Dictionary<WorkDay,Tuple<TimeOnly,int>> Schedule { get; set; }
        public DateTime Start { get; set; }
        public bool Online { get; set; }
        public int MaxStudents { get; set; }
        public int NumStudents { get; set; }
        public CourseState State { get; set; }
        public int NumPenaltyPts { get; set; }
        public int NumStudentsPassed { get; set; }
        public double ReadingAvgScore { get; set; }
        public double WritingAvgScore { get; set; }
        public double ListeningAvgScore { get; set; }
        public double SpeakingAvgScore { get; set; }

        

        public Course(string name, Language language, LanguageLvl level, int duration, Dictionary<WorkDay, Tuple<TimeOnly, int>> schedule, DateTime start, bool online, int numStudents, CourseState state, int maxStudents = 0)
        {
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
            //set default values for attributes for reports
            NumPenaltyPts = 0;
            NumStudentsPassed = 0;
            ReadingAvgScore = 0;
            WritingAvgScore = 0;
            ListeningAvgScore = 0;
            SpeakingAvgScore = 0;
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
