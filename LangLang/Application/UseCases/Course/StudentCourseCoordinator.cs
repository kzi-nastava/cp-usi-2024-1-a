using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using LangLang.Application.DTO;
using LangLang.Application.Stores;
using LangLang.Application.UseCases.DropRequest;
using LangLang.Application.UseCases.User;
using LangLang.Application.Utility.Authentication;
using LangLang.Application.Utility.Notification;
using LangLang.Domain.Model;
using static LangLang.Domain.Model.CourseApplication;

namespace LangLang.Application.UseCases.Course
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

        public void FinishCourse(string courseId, ObservableCollection<Student> students)
        {
            Domain.Model.Course course = _courseService.GetCourseById(courseId)!;
            _courseService.FinishCourse(courseId);
            foreach (Student student in students)
            {
                _studentService.AddLanguageSkill(student, course.Language, course.Level) ;
                _courseApplicationService.ActivateStudentApplications(student.Id);
            }
        }

        public List<Domain.Model.Course> GetAvailableCourses(string studentId)
        {
            List<Domain.Model.Course> availableCourses = _courseService.GetAvailableCourses(_studentService.GetStudentById(studentId)!);
            CourseAttendance studentAttendance = _courseAttendanceService.GetStudentAttendance(studentId)!;
            if(studentAttendance != null)
            {
                availableCourses.Remove(_courseService.GetCourseById(studentAttendance.CourseId)!);
            }

            foreach(Domain.Model.Course appliedCourse in GetAppliedCoursesStudent(studentId))
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

        public List<Domain.Model.Course> GetAppliedCoursesStudent(string studentId)
        {
            List<CourseApplication> applications = _courseApplicationService.GetApplicationsForStudent(studentId);
            List<Domain.Model.Course> appliedCourses = new();
            foreach(CourseApplication application in applications)
            {
                appliedCourses.Add(_courseService.GetCourseById(application.CourseId)!);
            }
            return appliedCourses;
        }

        public Domain.Model.Course? GetStudentAttendingCourse(string studentId)
        {
            CourseAttendance courseAttendance = _courseAttendanceService.GetStudentAttendance(studentId)!;
            if (courseAttendance == null) return null;
            return _courseService.GetCourseById(courseAttendance.CourseId);
        }
        public List<Domain.Model.Course> GetFinishedAndGradedCourses()
        {
            List<string> finishedCourseIds = _courseAttendanceService.GetFinishedAndGradedCourseIds();
            List<Domain.Model.Course> finishedCourses = new();
            foreach (string courseId in finishedCourseIds)
            {
                finishedCourses.Add(_courseService.GetCourseById(courseId)!);             
            }
            return finishedCourses;
        }

        public List<Domain.Model.Course> GetFinishedCourses()
        {
            List<CourseAttendance> attendances = _courseAttendanceService.GetFinishedCourses();
            List<Domain.Model.Course> finishedCourses = new();
            foreach (CourseAttendance attendance in attendances)
            {
                finishedCourses.Add(_courseService.GetCourseById(attendance.CourseId)!);
            }
            return finishedCourses;
        }

        public List<Domain.Model.Course> GetFinishedCoursesStudent(string studentId)
        {
            List<CourseAttendance> attendances = _courseAttendanceService.GetFinishedCoursesStudent(studentId);
            List<Domain.Model.Course> finishedCourses = new();
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
                    bool studentAttendingAnotherCourse = (_courseAttendanceService.GetStudentAttendance(application.StudentId)) != null;
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
            Domain.Model.Course? course = _courseService.GetCourseById(courseId);
            if (course == null)
            {
                return false;
            }
            return course.CanBeModified();
        }

        public bool CanDropCourse(string courseId)
        {
            Domain.Model.Course? course = _courseService.GetCourseById(courseId);
            if (course == null)
            {
                return false;
            }
            return course.State == Domain.Model.Course.CourseState.InProgress;
        }

        public void AcceptDropRequest(Domain.Model.DropRequest dropRequest)
        {
            _dropRequestService.Accept(dropRequest);
            _courseApplicationService.ActivateStudentApplications(dropRequest.SenderId);
        }

        public void DenyDropRequest(Domain.Model.DropRequest dropRequest)
        {
            _dropRequestService.Deny(dropRequest);
            _courseApplicationService.ActivateStudentApplications(dropRequest.SenderId);
        }

        public List<CourseAttendance> GetGradedAttendancesForLastYear()
        {
            List<CourseAttendance> attendances = new();
            var courses = _courseService.GetCoursesForLastYear();
            foreach (var course in courses)
            {
                attendances.AddRange(_courseAttendanceService.GetAttendancesForCourse(course.Id)
                    .Where(attendance => attendance.IsGraded));
            }
            return attendances;
        }

        public void RemoveCoursesOfTutor(Tutor tutor)
        {
            foreach (var course in _courseService.GetCoursesByTutor(tutor))
            {
                if (course.IsCreatedByTutor && course.CanBeModified())
                {
                    _courseApplicationService.RemoveCourseApplications(course.Id);
                    _courseAttendanceService.RemoveCourseAttendances(course.Id);
                    _courseService.DeleteCourse(course.Id);
                }
            }
        }
    }
}
