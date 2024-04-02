using LangLang.DAO;
using LangLang.Model;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using Consts;

namespace LangLang.Services
{
    internal class CourseService
    {
        CourseDAO courseDAO = CourseDAO.GetInstance();
        LanguageDAO languageDAO = LanguageDAO.GetInstance();

        public Dictionary<string,Course> GetAll()
        {
            return courseDAO.GetAllCourses();
        }

        public void AddCourse(Course course)
        {
            courseDAO.AddCourse(course);
        }

        public Course GetCourseById(string id)
        {
            return courseDAO.GetCourseById(id);
        }

        public void DeleteCourse(string id)
        {
            courseDAO.DeleteCourse(id);
        }

        public void UpdateCourse(Course course)
        {
            courseDAO.UpdateCourse(course);
        }

        public Course? ValidateInputs(string name, string? languageName, LanguageLvl? level, int? duration, Dictionary<WorkDay,Tuple<TimeOnly,int>> schedule,ObservableCollection<WorkDay> scheduleDays, string start, bool online, int numStudents, CourseState? state, int maxStudents)
        {
            if (name == "" || languageName == null || duration == null || scheduleDays.Count == 0 || start == "" || maxStudents == 0 || numStudents == 0 || level == null || state == null)
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
                        DateTime.Parse(start),
                        online,
                        numStudents,
                        (CourseState)state,
                        maxStudents



                );
            }
        }
    }
}
