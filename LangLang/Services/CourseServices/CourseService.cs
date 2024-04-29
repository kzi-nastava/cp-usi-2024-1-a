using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Consts;
using LangLang.DAO;
using LangLang.Model;

namespace LangLang.Services.CourseServices
{
    public class CourseService : ICourseService
    {
        private readonly ICourseDAO _courseDao;
        private readonly ILanguageDAO _languageDao;
        private readonly ITutorDAO _tutorDao;

        public CourseService(ICourseDAO courseDao, ILanguageDAO languageDao, ITutorDAO tutorDao)
        {
            _courseDao = courseDao;
            _languageDao = languageDao;
            _tutorDao = tutorDao;
        }


        public Dictionary<string, Course> GetAll()
        {
            return _courseDao.GetAllCourses();
        }
        public Dictionary<string, Course> GetCoursesByTutor(Tutor loggedInUser)
        {
            Dictionary<string, Course> courses = new();
            foreach (string courseId in loggedInUser.Courses)
            {
                courses.Add(courseId, _courseDao.GetCourseById(courseId)!);
            }
            return courses;
        }
        public List<Course> GetAvailableCourses(Student student)
        {
            List<Course> courses = new();
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
                courses.Add(course);
            }

            return courses;
        }

        public void AddCourse(Course course, Tutor loggedInUser)
        {
            _courseDao.AddCourse(course);
            loggedInUser.Courses.Add(course.Id);
            _tutorDao.UpdateTutor(loggedInUser);
        }

        public Course? GetCourseById(string id)
        {
            return _courseDao.GetCourseById(id);
        }

        public void DeleteCourse(string id, Tutor loggedInUser)
        {
            loggedInUser.Courses.Remove(id);
            _courseDao.DeleteCourse(id);
            _tutorDao.UpdateTutor(loggedInUser);
        }

        public void UpdateCourse(Course course)
        {
            _courseDao.UpdateCourse(course);
        }
        public void FinishCourse(string id)
        {
            Course? course = GetCourseById(id);
            if (course == null)
            {
                throw new ArgumentException("Course not found");
            }
            course.State = CourseState.Finished;
            UpdateCourse(course);
        }
        public void CalculateAverageScores(string id)
        {
            Course? course = GetCourseById(id);
            if (course == null)
            {
                throw new ArgumentException("Course not found");
            }
            // TODO: think about placing logic for calculating results

        }

        public void AddAttendance(string courseId)
        {
            GetCourseById(courseId)!.AddAttendance();
        }

        public void CancelAttendance(string courseId)
        {
            GetCourseById(courseId)!.CancelAttendance();
        }


        public Course? ValidateInputs(string name, string? languageName, LanguageLvl? level, int? duration, Dictionary<WorkDay, Tuple<TimeOnly, int>> schedule, ObservableCollection<WorkDay> scheduleDays, DateTime? start, bool online, int numStudents, CourseState? state, int maxStudents)
        {
            if (name == "" || languageName == null || duration == null || scheduleDays.Count == 0 || maxStudents == 0 || level == null || state == null)
            {
                return null;
            }
            if (start == null)
            {
                return null;
            }
            else
            {
                Language? language = _languageDao.GetLanguageById(languageName);
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
                    (DateTime)start,
                    online,
                    numStudents,
                    (CourseState)state,
                    maxStudents
                );
            }
        }
    }
}
