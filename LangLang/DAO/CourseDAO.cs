using System;
using Consts;
using System.Collections.Generic;
using LangLang.Model;
using LangLang.Util;

namespace LangLang.DAO
{
    internal class CourseDAO
    {
        private static CourseDAO instance;
        private Dictionary<string, Course> courses;

        private CourseDAO()
        {

        }
        public static CourseDAO getInstance()
        {
            if(instance == null)
            {
                instance = new CourseDAO();
            }
            return instance;
        }

        public Dictionary<string, Course> getAllCourses()
        {
            if(courses == null)
            {
                courses = JsonUtil.ReadFromFile<Course>(Constants.CourseFilePath);
            }
            return courses;
        }

        public void AddCourse(Course course)
        {
            courses[course.Name] = course;
            JsonUtil.WriteToFile<Course>(courses, Constants.CourseFilePath);
        }

        public void DeleteStudent(string name)
        {
            courses.Remove(name);
        }


    }
}
