﻿using System;
using System.Collections.Generic;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;
using LangLang.Domain.Utility;

namespace LangLang.Repositories.Json
{
    public class CourseRepository : AutoIdRepository<Course>, ICourseRepository
    {
        public CourseRepository(string filepath, string lastIdFilePath) : base(filepath, lastIdFilePath)
        {
        }
        
        public List<Course> GetCoursesByDate(DateOnly date)
        {
            List<Course> courses = new();
            foreach (Course course in GetAll().Values)
            {
                if (
                    date >= DateOnly.FromDateTime(course.Start) &&
                    date <= DateOnly.FromDateTime(course.Start.Add(TimeSpan.FromDays(7*course.Duration))) &&
                    date.DayOfWeek!=DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday && 
                    course.Schedule.ContainsKey(DayConverter.ToWorkDay(date.DayOfWeek))
                    )
                {
                    courses.Add(course);
                }
            }

            return courses;
        }
    }
}
