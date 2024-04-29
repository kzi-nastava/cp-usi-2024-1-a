using Consts;
using LangLang.Model;
using LangLang.Services.StudentCourseServices;
using LangLang.Services.UserServices;
using System;
using System.Collections.Generic;
using static LangLang.Model.CourseApplication;

namespace LangLang.Services.CourseServices
{
    public class StudentCourseCoordinator : IStudentCourseCoordinator
    {
        private readonly ICourseService _courseService;
        private readonly IStudentService _studentService;
        private readonly ICourseApplicationService _courseApplicationService;
        private readonly ICourseAttendanceService _courseAttendanceService;

        public StudentCourseCoordinator(ICourseService courseService,ICourseAttendanceService courseAttendanceService, IStudentService studentService, ICourseApplicationService courseApplicationService)
        {
            _courseService = courseService;
            _courseAttendanceService = courseAttendanceService;
            _studentService = studentService;
            _courseApplicationService = courseApplicationService;
        }

        public void Accept(string applicationId)
        {
            CourseApplication? application = _courseApplicationService.GetCourseApplicationById(applicationId);
            if (application == null)
            {
                throw new ArgumentException("No application found");
            }
            if (!CanBeModified(application.CourseId))
            {
                throw new ArgumentException("Cannot accept student at this state");
            }
            _courseApplicationService.ChangeApplicationState(application.Id, State.Accepted);
            _courseApplicationService.PauseStudentApplications(application.StudentId);
            Course? course = _courseService.GetCourseById(application.CourseId);
            course!.AddAttendance();
        }

        public void ApplyForCourse(string courseId, string studentId)
        {
            // if courseAttendingService.getAttendingCourseForStudent(studentid) != null throw exception
            CourseApplication application = _courseApplicationService.ApplyForCourse(studentId, courseId);

        }

        public void CancelAplication(string applicationId)
        {
            CourseApplication? application = _courseApplicationService.GetCourseApplicationById(applicationId);
            if (application == null)
            {
                throw new ArgumentException("No application found");
            }
            if (!CanBeModified(application.CourseId))
            {
                throw new ArgumentException("Cannot accept student at this state");
            }
            _courseApplicationService.ChangeApplicationState(applicationId, State.Rejected);
            _courseApplicationService.ActivateStudentApplications(application.StudentId);
        }

        public void DropCourse(string courseId)
        {
            throw new NotImplementedException();
        }

        public void FinishCourse(string courseId, string studentId)
        {
            //_courseService.CalculateAverageScores
            _courseService.FinishCourse(courseId);
            _courseApplicationService.ActivateStudentApplications(studentId);
        }

        public void GenerateAttendance()
        {
            throw new NotImplementedException();
        }

        public void RemoveAttendee(string courseId, string studentId)
        {
            // TODO: Add courseAttendance.RemoveAttendee method here.
            List<CourseApplication> applications = _courseApplicationService.GetApplicationsForStudent(studentId);
            foreach (CourseApplication application in applications)
            {
                if (application.CourseApplicationState == State.Accepted)
                {
                    _courseApplicationService.RejectApplication(application.Id);
                }
            }

            Course? course = _courseService.GetCourseById(courseId);
            course!.CancelAttendance();
        }

        public bool CanBeModified(string courseId)
        {
            Course? course = _courseService.GetCourseById(courseId);
            if (course == null)
            {
                return false;
            }
            if (course.Start - Constants.ConfirmableCourseTime < DateTime.Now)
            {
                return false;
            }
            return true;
        }
    }
}
