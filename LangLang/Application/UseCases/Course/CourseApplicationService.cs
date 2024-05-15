using System;
using System.Collections.Generic;
using LangLang.Application.UseCases.User;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;
using static LangLang.Domain.Model.CourseApplication;

namespace LangLang.Application.UseCases.Course
{
    public class CourseApplicationService : ICourseApplicationService
    {
        private readonly ICourseService _courseService;
        private readonly IStudentService _studentService;
        private readonly ICourseApplicationRepository _courseApplicationRepository;

        public CourseApplicationService(ICourseService courseService, IStudentService studentService, ICourseApplicationRepository courseApplicationRepository)
        {
            _courseService = courseService;
            _studentService = studentService;
            _courseApplicationRepository = courseApplicationRepository;
        }

        //after accepting one course all other applications are paused,
        //when accepted course finishes this method is called to set all values back to pending from paused.
        public void ActivateStudentApplications(string studentId)
        {
            List<CourseApplication> applications = _courseApplicationRepository.GetForStudent(studentId);
            foreach (CourseApplication application in applications)
            {
                application.ChangeApplicationState(State.Pending);
            }
        }

        public CourseApplication ApplyForCourse(string studentId, string courseId)
        {
            Domain.Model.Course? course = _courseService.GetCourseById(courseId);
            if (course == null) throw new ArgumentException("No existing course.");
            if (course.IsFull()) throw new ArgumentException("The course is full.");
            CourseApplication application = new CourseApplication(studentId, courseId, State.Pending);
            _courseService.AddAttendance(courseId);
            _courseApplicationRepository.Add(application);
            return application;
        }

        public CourseApplication ChangeApplicationState(string applicationId, State state)
        {
            CourseApplication? application = _courseApplicationRepository.Get(applicationId);
            if (application == null)
            {
                throw new ArgumentException("Course application not found");
            }
            application.ChangeApplicationState(state);
            _courseApplicationRepository.Update(applicationId, application);
            return application;
        }
        public void DeleteApplication(string applicationId)
        {
            _courseApplicationRepository.Delete(applicationId);
        }

        public List<CourseApplication> GetApplicationsForCourse(string courseId)
        {
            return _courseApplicationRepository.GetForCourse(courseId);
        }

        public List<CourseApplication> GetApplicationsForStudent(string studentId)
        {
            return _courseApplicationRepository.GetForStudent(studentId);
        }

        public CourseApplication? GetCourseApplicationById(string id)
        {
            return _courseApplicationRepository.Get(id);
        }

        public CourseApplication? GetApplication(string studentId, string courseId)
        {
            List<CourseApplication> applications = _courseApplicationRepository.GetForStudent(studentId);
            foreach (CourseApplication application in applications)
            {
                if (application.CourseId == courseId)
                {
                    return application;
                }
            }
            return null;
        }

        public void PauseStudentApplications(string studentId)
        {
            List<CourseApplication> applications = _courseApplicationRepository.GetForStudent(studentId);
            foreach (CourseApplication application in applications)
            {
                if (application.CourseApplicationState != State.Accepted)
                {
                    application.ChangeApplicationState(State.Paused);
                    _courseApplicationRepository.Update(application.Id, application);
                }
            }
        }

        public void RejectApplication(string applicationId)
        {
            CourseApplication? application = _courseApplicationRepository.Get(applicationId);
            if (application == null)
            {
                throw new ArgumentException("Course application not found");
            }
            Domain.Model.Course? course = _courseService.GetCourseById(application.CourseId);
            if (course == null)
            {
                throw new ArgumentException("Cannot find course.");
            }
            application.ChangeApplicationState(State.Rejected);
        }

        public void CancelApplication(string applicationId)
        {
            CourseApplication? application = GetCourseApplicationById(applicationId);
            if (application!.CourseApplicationState == State.Accepted)
            {
                ActivateStudentApplications(application.StudentId);
                _courseService.CancelAttendance(application.CourseId);
            }
            DeleteApplication(applicationId);
        }
        public void RemoveStudentApplications(string studentId)
        {
            List<CourseApplication> applications = _courseApplicationRepository.GetForStudent(studentId);
            foreach (CourseApplication application in applications)
            {
                if (application.CourseApplicationState == State.Accepted)
                {
                    _courseService.CancelAttendance(application.CourseId);
                }
                DeleteApplication(application.Id);
            }
        }

        public List<CourseApplication> GetPendingApplicationsForCourse(string courseId)
        {
            List<CourseApplication> applications = new();
            foreach(CourseApplication application in _courseApplicationRepository.GetForCourse(courseId))
            {
                if(application.CourseApplicationState == State.Pending)
                {
                    applications.Add(application);
                }
            }
            return applications;
        }
    }
}
