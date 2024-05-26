using System;
using System.Collections.Generic;
using LangLang.Application.UseCases.User;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;

namespace LangLang.Application.UseCases.Course
{
    public class CourseAttendanceService : ICourseAttendanceService
    {
        private readonly ICourseService _courseService;
        private readonly ITutorService _tutorService;
        private readonly ICourseAttendanceRepository _courseAttendanceRepository;


        public CourseAttendanceService(ICourseService courseService, ITutorService tutorService, ICourseAttendanceRepository courseAttendanceRepository)
        {
            _courseService = courseService;
            _tutorService = tutorService;
            _courseAttendanceRepository = courseAttendanceRepository;
        }
        public CourseAttendance? GetAttendance(string studentId, string courseId)
        {
            List<CourseAttendance> attendances = _courseAttendanceRepository.GetForStudent(studentId);
            foreach (CourseAttendance attendance in attendances)
            {
                Domain.Model.Course course = _courseService.GetCourseById(attendance.CourseId)!;
                if (course.Id == courseId)
                {
                    return attendance;
                }
            }
            return null;
        }
        public List<CourseAttendance> GetAllStudentAttendances(string studentId)
        {
            return _courseAttendanceRepository.GetForStudent(studentId);
        }

        public List<CourseAttendance> GetAttendancesForCourse(string courseId)
        {
            return _courseAttendanceRepository.GetForCourse(courseId);
        }

        public CourseAttendance? GetStudentAttendance(string studentId)
        {
            List<CourseAttendance> attendances = _courseAttendanceRepository.GetForStudent(studentId);

            foreach (CourseAttendance attendance in attendances)
            {
                Domain.Model.Course course = _courseService.GetCourseById(attendance.CourseId)!;
                if (course.IsActive())
                {
                    return attendance;
                }
            }
            return null;
        }
        public List<CourseAttendance> GetFinishedCourses()
        {
            List<CourseAttendance> allAttendances = _courseAttendanceRepository.GetAll();
            List<CourseAttendance> finishedAttendances = new();
            foreach (CourseAttendance attendance in allAttendances)
            {
                Domain.Model.Course course = _courseService.GetCourseById(attendance.CourseId)!;
                if (course.IsFinished())
                    finishedAttendances.Add(attendance);
            }
            return finishedAttendances;
        }

        public List<CourseAttendance> GetFinishedAndGradedCourses()
        {
            List<CourseAttendance> allAttendances = _courseAttendanceRepository.GetAll();
            List<CourseAttendance> finishedAttendances = new();
            foreach (CourseAttendance attendance in allAttendances)
            {
                Domain.Model.Course course = _courseService.GetCourseById(attendance.CourseId)!;
                if (course.IsFinishedAndGraded())
                    finishedAttendances.Add(attendance);
            }
            return finishedAttendances;
        }

        public List<CourseAttendance> GetFinishedCoursesStudent(string studentId)
        {
            List<CourseAttendance> allAttendances = _courseAttendanceRepository.GetForStudent(studentId);
            List<CourseAttendance> finishedAttendances = new();
            foreach (CourseAttendance attendance in allAttendances)
            {
                Domain.Model.Course course = _courseService.GetCourseById(attendance.CourseId)!;
                if (course.IsFinished())
                    finishedAttendances.Add(attendance);
            }
            return finishedAttendances;
        }

        public CourseAttendance AddAttendance(string studentId, string courseId)
        {
            CourseAttendance attendance = new CourseAttendance(courseId, studentId, false, false,0, 0);
            _courseAttendanceRepository.Add(attendance);
            return attendance;
        }

        public void RemoveAttendee(string studentId, string courseId)
        {
            List<CourseAttendance> attendances = _courseAttendanceRepository.GetForStudent(studentId);
            foreach (CourseAttendance attendance in attendances)
            {
                if (attendance.CourseId == courseId)
                {
                    if (_courseService.GetCourseById(courseId)!.State != Domain.Model.Course.CourseState.NotStarted)
                    {
                        _courseService.CancelAttendance(courseId);
                    }
                    _courseAttendanceRepository.Delete(attendance.Id);
                }
            }
        }

        public CourseAttendance? GradeStudent(string studentId, string courseId, int knowledgeGrade, int activityGrade)
        {
            CourseAttendance? attendance = _courseAttendanceRepository.GetStudentAttendanceForCourse(studentId, courseId);
            if (attendance == null) return null;
            attendance.AddGrade(activityGrade, knowledgeGrade);
            _courseAttendanceRepository.Update(attendance.Id, attendance);
            return attendance;
        }

        public void AddPenaltyPoint(string studentId, string courseId)
        {
            var attendance = GetAttendance(studentId, courseId) ??
                             throw new ArgumentException("No attendance for the given student on the given course");
            attendance.AddPenaltyPoint();
            _courseAttendanceRepository.Update(attendance.Id, attendance);
        }

        public void RemoveCourseAttendances(string courseId)
        {
            foreach (var attendance in GetAttendancesForCourse(courseId))
            {
                _courseAttendanceRepository.Delete(attendance.Id);
            }
        }

        public bool RateTutor(Domain.Model.Course course, string studentId, int rating)
        {
            CourseAttendance attendance = GetAttendance(studentId, course.Id)!;
            if (!attendance.IsRated)
            {
                attendance.AddRating();
                _courseAttendanceRepository.Update(attendance.Id, attendance);
                Tutor tutor = _tutorService.GetTutorById(course.TutorId!)!;
               _tutorService.AddRating(tutor, rating);
                return true;
            }
            return false;
        }

    }
}
