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
        private static CourseDAO? instance;
        private Dictionary<string, Course>? courses;
        private Dictionary<string, Course> Courses
        {
            get { 
                if(courses == null)
                {
                    Load();
                } 
                return courses!;
            }
            set { courses = value; }
        }
        

        private static LastIdDAO lastIdDAO = LastIdDAO.GetInstance();

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
                courses = JsonUtil.ReadFromFile<Course>(Constants.CourseFilePath);
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
