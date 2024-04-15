using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Consts;
using LangLang.DAO;
using LangLang.Model;

namespace LangLang.Services.EntityServices
{
    public class CourseService : ICourseService
    {
        CourseDAO courseDAO = CourseDAO.GetInstance();
        LanguageDAO languageDAO = LanguageDAO.GetInstance();
        TutorDAO tutorDAO = TutorDAO.GetInstance();

        public Dictionary<string,Course> GetAll()
        {
            return courseDAO.GetAllCourses();
        }
        public Dictionary<string, Course> GetCoursesByTutor(Tutor loggedInUser)
        {
            Dictionary<string, Course> courses = new();
            foreach(string courseId in loggedInUser.Courses)
            {
                courses.Add(courseId, courseDAO.GetCourseById(courseId)!);
            }
            return courses;
        }
        public List<Course> GetAvailableCourses(Student student)
        {
            List<Course> Courses = new();
            foreach (Course course in GetAll().Values.ToList())
            {
                if (course.State != CourseState.Active)
                {
                    continue;
                }
                if (course.IsFull())
                {
                    continue;
                }
                Courses.Add(course);
            }

            return Courses;
        }

        public void AddCourse(Course course, Tutor loggedInUser)
        {
            courseDAO.AddCourse(course);
            loggedInUser.Courses.Add(course.Id);
            tutorDAO.UpdateTutor(loggedInUser);
        }

        public Course? GetCourseById(string id)
        {
            return courseDAO.GetCourseById(id);
        }

        public void DeleteCourse(string id, Tutor loggedInUser)
        {
            loggedInUser.Courses.Remove(id);
            courseDAO.DeleteCourse(id);
            tutorDAO.UpdateTutor(loggedInUser);
        }

        public void UpdateCourse(Course course)
        {
            courseDAO.UpdateCourse(course);
        }

        public Course? ValidateInputs(string name, string? languageName, LanguageLvl? level, int? duration, Dictionary<WorkDay,Tuple<TimeOnly,int>> schedule,ObservableCollection<WorkDay> scheduleDays, string start, bool online, int numStudents, CourseState? state, int maxStudents)
        {
            DateTime coursedate = DateTime.Parse(start);
            if (name == "" || languageName == null || duration == null || scheduleDays.Count == 0 || start == "" || maxStudents == 0 || level == null || state == null)
            {
                return null;
            }
            if (coursedate < DateTime.Now)
            {
                return null;
            }
            else
            {
                Language? language = languageDAO.GetLanguageById(languageName);
                if (language == null)
                {
                    return null;
                }
                if (online)
                {
                    maxStudents = int.MaxValue;
                }
                return new Course(
                        name,
                        language,
                        (LanguageLvl)level,
                        (int)duration,
                        schedule,
                        coursedate,
                        online,
                        numStudents,
                        (CourseState)state,
                        maxStudents



                );
            }
        }
    }
}
