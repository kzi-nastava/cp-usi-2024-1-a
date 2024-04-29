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
        private readonly ILastIdDAO _lastIdDAO;

        public CourseApplicationService(ICourseService courseService, IStudentService studentService, ICourseApplicationDAO courseApplicationDAO, ILastIdDAO lastIdDAO)
        {
            _courseService = courseService;
            _studentService = studentService;
            _courseApplicationDAO = courseApplicationDAO;
            _lastIdDAO = lastIdDAO;
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
            CourseApplication application = new CourseApplication(id, studentId, courseId, State.Pending);
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

        public void RemoveStudentApplications(string studentId)
        {
            List<CourseApplication> applications = _courseApplicationDAO.GetCourseApplicationsForStudent(studentId);
            foreach (CourseApplication application in applications)
            {
                // NOTE: not sure if i should delete CourseApplication or just change state.
                //application.ChangeApplicationState(State.Paused);
                DeleteApplication(application.Id);
            }
        }
    }
}
