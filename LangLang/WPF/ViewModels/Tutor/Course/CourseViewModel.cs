using LangLang.Domain.Model;
using System;
using System.Collections.Generic;
using static LangLang.Domain.Model.Course;

namespace LangLang.WPF.ViewModels.Tutor.Course
{
    public class CourseViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string LanguageName { get; set; }
        public string Level { get; set; }
        public int Duration { get; set; }
        public ScheduleViewModel Schedule { get; set; }
        public DateTime Start { get; set; }
        public bool Online { get; set; }
        public int MaxStudents { get;set; }
        public int NumStudents { get; set; }
        public CourseState State { get; set; }

        public CourseViewModel(Domain.Model.Course course): this(
            course.Id,
            course.Name,
            course.Language.Name,
            course.Level.ToString(),
            course.Duration,
            course.Schedule,
            course.Start,
            course.Online,
            course.MaxStudents,
            course.NumStudents,
            course.State
            )
        {
        }

        public CourseViewModel(string id, string name, string languageName, string level, int duration, Dictionary<WorkDay, Tuple<TimeOnly, int>> schedule, DateTime start, bool online, int maxStudents, int numStudents, CourseState state)
        {
            Id = id;
            Name = name;
            LanguageName = languageName;
            Level = level;
            Duration = duration;
            Schedule = new ScheduleViewModel(schedule);
            Start = start;
            Online = online;
            MaxStudents = maxStudents;
            NumStudents = numStudents;
            State = state;
        }

    }
}
