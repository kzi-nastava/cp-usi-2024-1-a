using System;
using Consts;
using System.Collections.Generic;
using LangLang.Model;
using LangLang.Util;
using System.IO;

namespace LangLang.DAO
{
    internal class CourseDAO
    {
        private static CourseDAO? _instance;
        private Dictionary<string, Course>? _courses;
        private Dictionary<string, Course> Courses
        {
            get { 
                if(_courses == null)
                {
                    Load();
                } 
                return _courses!;
            }
            set { _courses = value; }
        }
        

        private static LastIdDAO lastIdDAO = LastIdDAO.GetInstance();

        private CourseDAO()
        {
        }
        public static CourseDAO GetInstance()
        {
            if(_instance == null)
            {
                _instance = new CourseDAO();
            }
            return _instance;
        }

        public Dictionary<string, Course> GetAllCourses()
        {
            return Courses;
        }

        public void AddCourse(Course course)
        {
            string id = lastIdDAO.GetCourseId();
            lastIdDAO.IncrementCourseId();
            course.Id = id;
            Courses[id] = course;
            Save(); 
        }
        
        public Course? GetCourseById(string id)
        {
            try
            {
                return Courses[id];
            }
            catch(KeyNotFoundException)
            {

                return null;
            }
        }

        public List<Course> GetCoursesByDate(DateOnly date)
        {
            List<Course> courses = new();
            foreach (Course course in GetAllCourses().Values)
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

        public void DeleteCourse(string id)
        {
            Courses.Remove(id);
            Save();
            
        }
        public void UpdateCourse(Course course)
        {
            Courses[course.Id] = course;
            Save();
            
        }

        private void Load()
        {
            try
            {
                _courses = JsonUtil.ReadFromFile<Course>(Constants.CourseFilePath);
            }catch(DirectoryNotFoundException)
            {
                Courses = new Dictionary<string, Course>();
                Save();
            }catch(FileNotFoundException)
            {
                Courses = new Dictionary<string, Course>();
                Save();
            }
        }
        private void Save()
        {
            JsonUtil.WriteToFile<Course>(Courses, Constants.CourseFilePath);
        }
    }
}
