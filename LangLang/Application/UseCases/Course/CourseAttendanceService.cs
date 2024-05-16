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
            /*CourseAttendance attendance = _courseAttendanceRepository.
            //<<<<<<< HEAD
            _courseAttendanceDAO.DeleteCourseAttendance(GetStudentAttendance(studentId)!.Id);
            //=======
            
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
        */
         }

        public CourseAttendance? GradeStudent(string studentId, string courseId, int knowledgeGrade, int activityGrade)
        {
            CourseAttendance? attendance = _courseAttendanceRepository.GetStudentAttendanceForCourse(studentId, courseId);
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

    }
}
