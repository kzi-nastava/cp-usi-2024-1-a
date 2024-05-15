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

<<<<<<< HEAD
        public CourseAttendanceService(ICourseService courseService, ITutorService tutorService, ICourseAttendanceDAO courseAttendanceDAO)
=======
        public CourseAttendanceService(ICourseService courseService, IStudentService studentService, ITutorService tutorService, ICourseAttendanceRepository courseAttendanceRepository)
>>>>>>> develop
        {
            _courseService = courseService;
            _tutorService = tutorService;
            _courseAttendanceRepository = courseAttendanceRepository;
        }

        public List<CourseAttendance> GetAllStudentAttendances(string studentId)
        {
<<<<<<< HEAD
            return _courseAttendanceDAO.GetAllCourseAttendancesForStudent(studentId);
=======
            return _courseAttendanceRepository.GetForStudent(studentId);
>>>>>>> develop
        }

        public List<CourseAttendance> GetAttendancesForCourse(string courseId)
        {
            return _courseAttendanceRepository.GetForCourse(courseId);
        }

        public CourseAttendance? GetStudentAttendance(string studentId)
        {
<<<<<<< HEAD
            List<CourseAttendance> attendances = _courseAttendanceDAO.GetAllCourseAttendancesForStudent(studentId);
=======
            List<CourseAttendance> attendances = _courseAttendanceRepository.GetForStudent(studentId);
>>>>>>> develop
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
       
        public List<CourseAttendance> GetFinishedCoursesStudent(string studentId)
        {
<<<<<<< HEAD
            List<CourseAttendance> allAttendances = _courseAttendanceDAO.GetAllCourseAttendancesForStudent(studentId);
=======
            List<CourseAttendance> allAttendances = _courseAttendanceRepository.GetForStudent(studentId);
>>>>>>> develop
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
<<<<<<< HEAD
            _courseAttendanceDAO.DeleteCourseAttendance(GetStudentAttendance(studentId)!.Id);
=======
            List<CourseAttendance> attendances = _courseAttendanceRepository.GetForStudent(studentId);
            foreach (CourseAttendance attendance in attendances)
            {
                if (attendance.CourseId == courseId)
                {
                    if(_courseService.GetCourseById(courseId)!.State != Domain.Model.Course.CourseState.NotStarted)
                    {
                        _courseService.CancelAttendance(courseId);
                    }
                    _courseAttendanceRepository.Delete(attendance.Id);
                }
            }
>>>>>>> develop
        }

        public CourseAttendance? GradeStudent(string studentId, string courseId, int knowledgeGrade, int activityGrade)
        {
<<<<<<< HEAD
            CourseAttendance? attendance = GetStudentAttendance(studentId);
=======
            CourseAttendance? attendance = _courseAttendanceRepository.GetStudentAttendanceForCourse(studentId, courseId);
>>>>>>> develop
            if (attendance == null) return null;
            attendance.AddGrade(activityGrade, knowledgeGrade);
            _courseAttendanceRepository.Update(attendance.Id, attendance);
            return attendance;
        }


        public bool RateTutor(string courseId, string studentId, int rating)
        {
            CourseAttendance attendance = GetAttendance(studentId, courseId)!;
            if (!attendance.IsRated)
            {
                attendance.AddRating();
                _courseAttendanceRepository.Update(attendance.Id, attendance);
                Tutor tutor = _tutorService.GetTutorForCourse(courseId)!;
               _tutorService.AddRating(tutor, rating);
                return true;
            }
            return false;
        }

        public CourseAttendance? GetAttendance(string studentId, string courseId)
        {
<<<<<<< HEAD
            List<CourseAttendance> attendances = _courseAttendanceDAO.GetAllCourseAttendancesForStudent(studentId);
=======
            List<CourseAttendance> attendances = _courseAttendanceRepository.GetForStudent(studentId);
>>>>>>> develop
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

    }
}
