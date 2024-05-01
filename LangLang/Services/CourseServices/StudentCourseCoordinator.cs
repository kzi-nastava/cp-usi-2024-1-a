using LangLang.Model;
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
            if (_courseAttendanceService.GetStudentAttendance(studentId) != null) {
                throw new ArgumentException("Applicant already enrolled in a class!");
            }
            _courseApplicationService.ApplyForCourse(studentId, courseId);
        }

        public void CancelApplication(string applicationId)
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
            _courseApplicationService.CancelApplication(applicationId);
        }

        public void CancelApplication(string studentId, string courseId)
        {
            CourseApplication? application = _courseApplicationService.GetApplication(studentId, courseId);
            if (application == null)
            {
                throw new ArgumentException("No application found");
            }
            if (!CanBeModified(application.CourseId))
            {
                throw new ArgumentException("Cannot accept student at this state");
            }
            _courseApplicationService.CancelApplication(application.Id);
        }

        public void DropCourse(string studentId)
        {
            CourseAttendance? attendance = _courseAttendanceService.GetStudentAttendance(studentId);
            if (attendance == null)
            {
                throw new ArgumentException("No attendance found");
            }
            if (!CanDropCourse(attendance.CourseId))
            {
                throw new ArgumentException("Cannot drop course this early on");
            }
            _courseService.CancelAttendance(attendance.CourseId);
            _courseApplicationService.ActivateStudentApplications(attendance.StudentId);
            _courseAttendanceService.RemoveAttendee(attendance.StudentId, attendance.CourseId);
            //Sent tutor the excuse why student wants to drop out
        }

        public void FinishCourse(string courseId, string studentId)
        {
            //_courseService.CalculateAverageScores
            _courseService.FinishCourse(courseId);
            //student service add language skill
            _courseApplicationService.ActivateStudentApplications(studentId);
        }

        public List<Course> GetAvailableCourses(string studentId)
        {
            List<Course> availableCourses = _courseService.GetAvailableCourses(_studentService.GetStudentById(studentId)!);
            CourseAttendance studentAttendance = _courseAttendanceService.GetStudentAttendance(studentId)!;
            if(studentAttendance != null)
            {
                availableCourses.Remove(_courseService.GetCourseById(studentAttendance.CourseId)!);
            }

            foreach(Course appliedCourse in GetAppliedCoursesStudent(studentId))
            {
                if (availableCourses.Contains(appliedCourse))
                {
                    availableCourses.Remove(appliedCourse);
                }
            }

            return availableCourses;
        }


        public List<Course> GetAppliedCoursesStudent(string studentId)
        {
            List<CourseApplication> applications = _courseApplicationService.GetApplicationsForStudent(studentId);
            List<Course> appliedCourses = new();
            foreach(CourseApplication application in applications)
            {
                appliedCourses.Add(_courseService.GetCourseById(application.CourseId)!);
            }
            return appliedCourses;
        }

        public Course? GetStudentAttendingCourse(string studentId)
        {
            CourseAttendance courseAttendance = _courseAttendanceService.GetStudentAttendance(studentId)!;
            if (courseAttendance == null) return null;
            return _courseService.GetCourseById(courseAttendance.CourseId);
        }

        public List<Course> GetFinishedCoursesStudent(string studentId)
        {
            List<CourseAttendance> attendances = _courseAttendanceService.GetFinishedCoursesStudent(studentId);
            List<Course> finishedCourses = new();
            foreach(CourseAttendance attendance in attendances)
            {
                finishedCourses.Add(_courseService.GetCourseById(attendance.CourseId)!);
            }
            return finishedCourses;
        }

        public void GenerateAttendance(string courseId)
        {
            List<CourseApplication> applications = _courseApplicationService.GetApplicationsForCourse(courseId);
            foreach(CourseApplication application in applications)
            {
                if(application.CourseApplicationState == State.Accepted)
                {
                    bool studentAttendingAnotherCourse = (_courseAttendanceService.GetAttendancesForStudent(application.StudentId)).Count != 0;
                    if (!studentAttendingAnotherCourse)
                    {
                        _courseAttendanceService.AddAttendance(application.StudentId, application.CourseId);
                        _courseApplicationService.DeleteApplication(application.Id);
                    }
                }
            }
        }

        public void RemoveAttendee(string studentId)
        {
            _courseApplicationService.RemoveStudentApplications(studentId);
            Course attendingCourse = GetStudentAttendingCourse(studentId)!;
            _courseAttendanceService.RemoveAttendee(studentId, attendingCourse.Id);
        }

        public bool CanBeModified(string courseId)
        {
            Course? course = _courseService.GetCourseById(courseId);
            if (course == null)
            {
                return false;
            }
            if (course.State != Course.CourseState.NotStarted)
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
            if(course.State != Course.CourseState.InProgress)
            {
                return false;
            }
            return true;
        }
    }
}
