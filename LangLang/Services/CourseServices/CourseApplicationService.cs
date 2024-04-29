using Consts;
using LangLang.DAO;
using LangLang.Model;
using LangLang.Services.UserServices;
using System;
using System.Collections.Generic;
using static LangLang.Model.CourseApplication;

namespace LangLang.Services.CourseServices
{
    public class CourseApplicationService : ICourseApplicationService
    {
        private readonly ICourseService _courseService;
        private readonly IStudentService _studentService;
        private readonly ICourseApplicationDAO _courseApplicationDAO;

        public CourseApplicationService(ICourseService courseService, IStudentService studentService, ICourseApplicationDAO courseApplicationDAO)
        {
            _courseService = courseService;
            _studentService = studentService;
            _courseApplicationDAO = courseApplicationDAO;
        }

        //after accepting one course all other applications are paused,
        //when accepted course finishes this method is called to set all values back to pending from paused.
        public void ActivateStudentApplications(string studentId)
        {
            List<CourseApplication> applications = _courseApplicationDAO.GetCourseApplicationsForStudent(studentId);
            foreach (CourseApplication application in applications)
            {
                application.ChangeApplicationState(State.Pending);
            }
        }

        public CourseApplication ApplyForCourse(string studentId, string courseId)
        {
            Course? course = _courseService.GetCourseById(courseId);
            if (course == null) throw new ArgumentException("No existing course.");
            if (course.IsFull()) throw new ArgumentException("The course is full.");
            CourseApplication application = new CourseApplication(studentId, courseId, State.Pending);
            _courseApplicationDAO.AddCourseApplication(application);
            return application;
        }

        public CourseApplication ChangeApplicationState(string applicationId, State state)
        {
            CourseApplication? application = _courseApplicationDAO.GetCourseApplicationById(applicationId);
            if (application == null)
            {
                throw new ArgumentException("Course application not found");
            }
            //if (!CanBeModified(application.CourseId))
            //{
            //    throw new ArgumentException("Course application state cannot be changed at this state.");
            //}
            application.ChangeApplicationState(state);
            return application;
        }
        public void DeleteApplication(string applicationId)
        {
            _courseApplicationDAO.DeleteCourseApplication(applicationId);
        }

        public List<CourseApplication> GetApplicationsForCourse(string courseId)
        {
            return _courseApplicationDAO.GetCourseApplicationsForCourse(courseId);
        }

        public List<CourseApplication> GetApplicationsForStudent(string studentId)
        {
            return _courseApplicationDAO.GetCourseApplicationsForStudent(studentId);
        }

        public CourseApplication? GetCourseApplicationById(string id)
        {
            return _courseApplicationDAO.GetCourseApplicationById(id);
        }

        public void PauseStudentApplications(string studentId)
        {
            List<CourseApplication> applications = _courseApplicationDAO.GetCourseApplicationsForStudent(studentId);
            foreach (CourseApplication application in applications)
            {
                if (application.CourseApplicationState != State.Accepted)
                {
                    application.ChangeApplicationState(State.Paused);
                }
            }
        }

        public void RejectApplication(string applicationId)
        {
            CourseApplication? application = _courseApplicationDAO.GetCourseApplicationById(applicationId);
            if (application == null)
            {
                throw new ArgumentException("Course application not found");
            }
            Course? course = _courseService.GetCourseById(application.CourseId);
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



        //TODO check where to place this
        //public bool CanBeModified(string courseId)
        //{
        //    Course? course = _courseService.GetCourseById(courseId);
        //    if (course == null)
        //    {
        //        return false;
        //    }
        //    if (course.Start - Constants.ConfirmableCourseTime < DateTime.Now)
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        public void RemoveStudentApplications(string studentId)
        {
            List<CourseApplication> applications = _courseApplicationDAO.GetCourseApplicationsForStudent(studentId);
            foreach (CourseApplication application in applications)
            {
                if (application.CourseApplicationState == State.Accepted)
                {
                    _courseService.CancelAttendance(application.CourseId);
                }
                DeleteApplication(application.Id);
            }
        }
    }
}
