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
            if (_courseAttendanceService.GetStudentAttendance(studentId) != null) {
                throw new ArgumentException("Applicant already enrolled in a class!");
            }
            _courseApplicationService.ApplyForCourse(studentId, courseId);
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
            if (application.CourseApplicationState == State.Accepted)
            {
                _courseApplicationService.ActivateStudentApplications(application.StudentId);
                _courseService.CancelAttendance(application.CourseId);
            }
            _courseApplicationService.DeleteApplication(application.Id);
        }

        public void DropCourse(string applicationId)
        {
            CourseApplication? application = _courseApplicationService.GetCourseApplicationById(applicationId);
            if (application == null)
            {
                throw new ArgumentException("No application found");
            }
            if (!CanDropCourse(application.CourseId))
            {
                throw new ArgumentException("Cannot drop course this early on");
            }
            _courseService.CancelAttendance(application.CourseId);
            _courseApplicationService.ActivateStudentApplications(application.StudentId);
            _courseAttendanceService.RemoveAttendee(application.StudentId, application.CourseId);
            //Sent tutor the excuse why student wants to drop out
        }

        public void FinishCourse(string courseId, string studentId)
        {
            //_courseService.CalculateAverageScores
            _courseService.FinishCourse(courseId);
            //student service add language skill
            _courseApplicationService.ActivateStudentApplications(studentId);
        }

        public void GenerateAttendance(string courseId)
        {
            List<CourseApplication> applications = _courseApplicationService.GetApplicationsForCourse(courseId);
            foreach(CourseApplication application in applications)
            {
                if(application.CourseApplicationState == State.Accepted)
                {
                    bool studentAttendingAnotherCourse = _courseAttendanceService.GetAttendancesForStudent(application.StudentId) != null;
                    if (!studentAttendingAnotherCourse)
                    {
                        _courseAttendanceService.AddAttendance(application.StudentId, application.CourseId);
                    }
                }
            }
        }

        public void RemoveAttendee(string courseId, string studentId)
        {
            List<CourseApplication> applications = _courseApplicationService.GetApplicationsForStudent(studentId);
            foreach (CourseApplication application in applications)
            {
                if (application.CourseApplicationState == State.Accepted)
                {
                    _courseService.CancelAttendance(application.CourseId);
                }
            }
            _courseApplicationService.RemoveStudentApplications(studentId);
            _courseAttendanceService.RemoveAttendee(studentId, courseId);
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

        public bool CanDropCourse(string courseId)
        {
            Course? course = _courseService.GetCourseById(courseId);
            if (course == null)
            {
                return false;
            }
            if (course.Start + Constants.CancellableCourseTime > DateTime.Now)
            {
                return false;
            }
            return true;
        }
    }
}
