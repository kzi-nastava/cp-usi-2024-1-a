using LangLang.DTO;
using LangLang.Model;
using LangLang.Services.AuthenticationServices;
using LangLang.Services.DropRequestServices;
using LangLang.Services.NotificationServices;
using LangLang.Services.UserServices;
using LangLang.Stores;
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
        private readonly IUserProfileMapper _userProfileMapper;
        private readonly INotificationService _notificationService;
        private readonly IAuthenticationStore _authenticationStore;
        private readonly IDropRequestService _dropRequestService;

        public StudentCourseCoordinator(ICourseService courseService,ICourseAttendanceService courseAttendanceService,
            IStudentService studentService, ICourseApplicationService courseApplicationService, IUserProfileMapper userProfileMapper,
            INotificationService notificationService, IAuthenticationStore authenticationStore, IDropRequestService dropRequestService)
        {
            _courseService = courseService;
            _authenticationStore = authenticationStore;
            _userProfileMapper = userProfileMapper;
            _courseAttendanceService = courseAttendanceService;
            _studentService = studentService;
            _courseApplicationService = courseApplicationService;
            _notificationService = notificationService;
            _dropRequestService = dropRequestService;
        }

        public void Accept(string studentId, string courseId)
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
            _courseApplicationService.PauseStudentApplications(application.StudentId);
            _courseAttendanceService.AddAttendance(application.StudentId, application.CourseId);
            _courseApplicationService.DeleteApplication(application.Id);
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
        public void SendNotification(string message, string receiverId)
        {
            if (message != null)
            {

                Profile? receiver = _userProfileMapper.GetProfile(new UserDto(_studentService.GetStudentById(receiverId), UserType.Student));
                Profile? sender = _authenticationStore.CurrentUserProfile;
                
                if (receiver == null)
                {
                    throw new ArgumentException("No receiver found");
                }
                _notificationService.AddNotification(message, receiver, sender);
            }
            
        }

        public void DropCourse(string studentId, string message)
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
            _dropRequestService.AddDropRequest(attendance.CourseId, _authenticationStore.CurrentUserProfile!, message);
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

        public List<Student> GetAppliedStudentsCourse(string courseId)
        {
            List<CourseApplication> applications = _courseApplicationService.GetPendingApplicationsForCourse(courseId);
            List<Student> appliedStudents = new();
            foreach (CourseApplication application in applications)
            {
                appliedStudents.Add(_studentService.GetStudentById(application.StudentId)!);
            }
            return appliedStudents;
        }

        public List<Student> GetAttendanceStudentsCourse(string courseId)
        {
            List<CourseAttendance> attendances = _courseAttendanceService.GetAttendancesForCourse(courseId);
            List<Student> attendanceStudents = new();
            foreach (CourseAttendance attendance in attendances)
            {
                attendanceStudents.Add(_studentService.GetStudentById(attendance.StudentId)!);
            }
            return attendanceStudents;
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
            var attendingCourse = GetStudentAttendingCourse(studentId);
            if (attendingCourse == null) return;
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

        public void AcceptDropRequest(DropRequest dropRequest)
        {
            _dropRequestService.Accept(dropRequest);
            _courseApplicationService.ActivateStudentApplications(dropRequest.SenderId);
        }

        public void DenyDropRequest(DropRequest dropRequest)
        {
            _dropRequestService.Deny(dropRequest);
            _courseApplicationService.ActivateStudentApplications(dropRequest.SenderId);
        }
    }
}
