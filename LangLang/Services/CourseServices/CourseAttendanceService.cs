using LangLang.DAO;
using LangLang.Model;
using LangLang.Services.UserServices;
using LangLang.Stores;
using System.Collections.Generic;


namespace LangLang.Services.CourseServices
{
    public class CourseAttendanceService : ICourseAttendanceService
    {
        private readonly ICourseService _courseService;
        private readonly IStudentService _studentService;
        private readonly ITutorService _tutorService;
        private readonly ICourseAttendanceDAO _courseAttendanceDAO;

        public CourseAttendanceService(ICourseService courseService, IStudentService studentService, ITutorService tutorService, ICourseAttendanceDAO courseAttendanceDAO)
        {
            _courseService = courseService;
            _studentService = studentService;
            _tutorService = tutorService;
            _courseAttendanceDAO = courseAttendanceDAO;
        }

        public List<CourseAttendance> GetAttendancesForStudent(string studentId)
        {
            return _courseAttendanceDAO.GetCourseAttendancesForStudent(studentId);
        }

        public List<CourseAttendance> GetAttendancesForCourse(string courseId)
        {
            return _courseAttendanceDAO.GetCourseAttendancesForCourse(courseId);
        }

        public CourseAttendance? GetStudentAttendance(string studentId)
        {
            List<CourseAttendance> attendances = _courseAttendanceDAO.GetCourseAttendancesForStudent(studentId);
            foreach (CourseAttendance attendance in attendances)
            {
                Course course = _courseService.GetCourseById(attendance.CourseId)!;
                if (course.State != Course.CourseState.NotStarted && course.State != Course.CourseState.FinishedGraded)
                {
                    return attendance;
                }
            }
            return null;
        }
        public List<CourseAttendance> GetFinishedCoursesStudent(string studentId)
        {
            List<CourseAttendance> allAttendances = _courseAttendanceDAO.GetCourseAttendancesForStudent(studentId);
            List<CourseAttendance> finishedAttendances = new();
            foreach (CourseAttendance attendance in allAttendances)
            {
                Course course = _courseService.GetCourseById(attendance.CourseId)!;
                if (course.State == Course.CourseState.FinishedGraded || course.State == Course.CourseState.FinishedNotGraded)
                    finishedAttendances.Add(attendance);
            }
            return finishedAttendances;
        }

        public CourseAttendance AddAttendance(string studentId, string courseId)
        {
            CourseAttendance attendance = new CourseAttendance(courseId, studentId, false, false,0, 0);
            _courseAttendanceDAO.AddCourseAttendance(attendance);
            return attendance;
        }

        public void RemoveAttendee(string studentId, string courseId)
        {
            List<CourseAttendance> attendances = _courseAttendanceDAO.GetCourseAttendancesForStudent(studentId);
            foreach (CourseAttendance attendance in attendances)
            {
                if (attendance.CourseId == courseId)
                {
                    if(_courseService.GetCourseById(courseId)!.State != Course.CourseState.NotStarted)
                    {
                        _courseService.CancelAttendance(courseId);
                    }
                    _courseAttendanceDAO.DeleteCourseAttendance(attendance.Id);
                }
            }
        }

        public void GradeStudent(string studentId, string CourseId, int knowledgeGrade, int activityGrade)
        {
            //predavac 6., i dont see this being used anywhere later on
        }


        public bool RateTutor(string courseId, string studentId, int rating)
        {
            CourseAttendance attendance = GetAttendance(studentId, courseId)!;
      
            if (!attendance.IsRated)
            {
                attendance.AddRating();
                _courseAttendanceDAO.UpdateCourseAttendance(attendance.Id, attendance);
                Tutor tutor = _tutorService.GetTutorForCourse(courseId)!;
               _tutorService.AddRating(tutor, rating);
                return true;
            }
            return false;
        }

        public CourseAttendance? GetAttendance(string studentId, string courseId)
        {
            List<CourseAttendance> attendances = _courseAttendanceDAO.GetCourseAttendancesForStudent(studentId);
            foreach (CourseAttendance attendance in attendances)
            {
                Course course = _courseService.GetCourseById(attendance.CourseId)!;
                if (course.Id == courseId)
                {
                    return attendance;
                }
            }
            return null;
        }

    }
}
