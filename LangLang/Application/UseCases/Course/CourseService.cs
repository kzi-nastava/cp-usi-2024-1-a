using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using LangLang.Domain;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;

namespace LangLang.Application.UseCases.Course
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly ITutorRepository _tutorRepository;

        public CourseService(ICourseRepository courseRepository, ILanguageRepository languageRepository, ITutorRepository tutorRepository)
        {
            _courseRepository = courseRepository;
            _languageRepository = languageRepository;
            _tutorRepository = tutorRepository;
        }


        public Dictionary<string, Domain.Model.Course> GetAll()
        {
            return _courseRepository.GetAll();
        }
        public Dictionary<string, Domain.Model.Course> GetCoursesByTutor(Tutor loggedInUser)
        {
            Dictionary<string, Domain.Model.Course> courses = new();
            foreach (string courseId in loggedInUser.Courses)
            {
                courses.Add(courseId, _courseRepository.Get(courseId)!);
            }
            return courses;
        }
        public List<Domain.Model.Course> GetAvailableCourses(Student student)
        {
            List<Domain.Model.Course> courses = new();
            foreach (Domain.Model.Course course in GetAll().Values.ToList())
            {
                if (course.State != Domain.Model.Course.CourseState.NotStarted)
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

        public void AddCourse(Domain.Model.Course course, Tutor loggedInUser)
        {
            _courseRepository.Add(course);
            loggedInUser.Courses.Add(course.Id);
            _tutorRepository.Update(loggedInUser.Id, loggedInUser);
        }

        public Domain.Model.Course? GetCourseById(string id)
        {
            return _courseRepository.Get(id);
        }

        public void DeleteCourse(string id, Tutor loggedInUser)
        {
            loggedInUser.Courses.Remove(id);
            _courseRepository.Delete(id);
            _tutorRepository.Update(loggedInUser.Id, loggedInUser);
        }

        public void UpdateCourse(Domain.Model.Course course)
        {
            _courseRepository.Update(course.Id, course);
        }

        public void FinishCourse(string id)
        {
            Domain.Model.Course? course = GetCourseById(id);
            if (course == null)
            {
                throw new ArgumentException("Course not found");
            }
            course.State = Domain.Model.Course.CourseState.FinishedGraded;
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
                Language? language = _languageRepository.Get(languageName);
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
                if(course.Start <= DateTime.Now && course.Start + course.Duration*Constants.CancellableCourseTime >= DateTime.Now)
                {
                    course.State = Domain.Model.Course.CourseState.InProgress;
                    UpdateCourse(course);
                }else if (course.Start - Constants.CancellableCourseTime < DateTime.Now)
                {
                    course.State = Domain.Model.Course.CourseState.Locked;
                    UpdateCourse(course);
                }
                else if(course.Start -Constants.CancellableCourseTime >= DateTime.Now)
                {
                    course.State = Domain.Model.Course.CourseState.NotStarted;
                    UpdateCourse(course);
                }
                else
                {
                    course.State = Domain.Model.Course.CourseState.FinishedNotGraded;
                    UpdateCourse(course);
                }
            }
        }
    }
}
