using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;

namespace LangLang.Application.UseCases.Course
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


        public Dictionary<string, Domain.Model.Course> GetAll()
        {
            return _courseDao.GetAllCourses();
        }
        public Dictionary<string, Domain.Model.Course> GetCoursesByTutor(Tutor loggedInUser)
        {
            return _courseDao.GetCoursesByTutor(loggedInUser);
        }
        public List<Domain.Model.Course> GetAvailableCourses(Student student)
        {
            List<Domain.Model.Course> courses = new();
            foreach (Domain.Model.Course course in GetAll().Values.ToList())
            {
                if(course.IsApplicable())
                {
                    courses.Add(course);
                }
            }

            return courses;
        }

        public void AddCourse(Domain.Model.Course course, Tutor loggedInUser)
        {
            _courseDao.AddCourse(course);
            loggedInUser.Courses.Add(course.Id);
            _tutorDao.UpdateTutor(loggedInUser);
        }

        public Domain.Model.Course? GetCourseById(string id)
        {
            return _courseDao.GetCourseById(id);
        }

        public void DeleteCourse(string id, Tutor loggedInUser)
        {
            loggedInUser.Courses.Remove(id);
            _courseDao.DeleteCourse(id);
            _tutorDao.UpdateTutor(loggedInUser);
        }

        public void UpdateCourse(Domain.Model.Course course)
        {
            _courseDao.UpdateCourse(course);
        }

        public void FinishCourse(string id)
        {
            Domain.Model.Course? course = GetCourseById(id);
            if (course == null)
            {
                throw new ArgumentException("Course not found");
            }
            course.Finish();
            UpdateCourse(course);
        }

        public void AddAttendance(string courseId)
        {
            GetCourseById(courseId)!.AddAttendance();
            UpdateCourse(GetCourseById(courseId)!);
        }

        public void CancelAttendance(string courseId)
        {
            GetCourseById(courseId)!.CancelAttendance();
            UpdateCourse(GetCourseById(courseId)!);
        }

        public Domain.Model.Course? ValidateInputs(string name, string? languageName, LanguageLevel? level, int? duration, Dictionary<WorkDay, Tuple<TimeOnly, int>> schedule, ObservableCollection<WorkDay> scheduleDays, DateTime? start, bool online, int numStudents, Domain.Model.Course.CourseState? state, int maxStudents)
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
                return new Domain.Model.Course(
                    name,
                    language,
                    (LanguageLevel)level,
                    (int)duration,
                    schedule,
                    (DateTime)start,
                    online,
                    numStudents,
                    (Domain.Model.Course.CourseState)state,
                    maxStudents
                );
            }
        }

        public void UpdateStates()
        {
            foreach(Domain.Model.Course course in GetAll().Values)
            {
                course.UpdateState();
                UpdateCourse(course);
            }
        }
    }
}
