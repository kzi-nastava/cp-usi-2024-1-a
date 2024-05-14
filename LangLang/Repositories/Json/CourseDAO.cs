using System;
using System.Collections.Generic;
using System.IO;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;
using LangLang.Repositories.Json.Util;

namespace LangLang.Repositories.Json
{
    public class CourseDAO : ICourseDAO
    {
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


        private readonly ILastIdDAO _lastIdDAO;

        public CourseDAO(ILastIdDAO lastIdDao)
        {
            _lastIdDAO = lastIdDao;
        }

        public Dictionary<string, Course> GetAllCourses()
        {
            return Courses;
        }

        public void AddCourse(Course course)
        {
            string id = _lastIdDAO.GetCourseId();
            _lastIdDAO.IncrementCourseId();
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

        public Dictionary<string, Course> GetCoursesByTutor(Tutor tutor)
        {
            Dictionary<string, Course> courses = new();
            foreach (string courseId in tutor.Courses)
            {
                courses.Add(courseId, GetCourseById(courseId)!);
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
