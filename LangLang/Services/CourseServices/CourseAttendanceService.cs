using LangLang.DAO;
using LangLang.Model;
using LangLang.Services.CourseServices;
using LangLang.Services.UserServices;
using System.Collections.Generic;


namespace LangLang.Services.StudentCourseServices
{
    public class CourseAttendanceService : ICourseAttendanceService
    {
        private readonly ICourseService _courseService;
        private readonly IStudentService _studentService;
        private readonly ITutorService _tutorService;
        private readonly ICourseAttendanceDAO _courseAttendanceDAO;
        private readonly ILastIdDAO _lastIdDAO;

        public CourseAttendanceService(ICourseService courseService, IStudentService studentService, ITutorService tutorService, ICourseAttendanceDAO courseAttendanceDAO, ILastIdDAO lastIdDAO)
        {
            _courseService = courseService;
            _studentService = studentService;
            _tutorService = tutorService;
            _courseAttendanceDAO = courseAttendanceDAO;
            _lastIdDAO = lastIdDAO;
        }


        public void RemoveAttendee(string studentId, string courseId)
        {
            List<CourseAttendance> attendances = _courseAttendanceDAO.GeCourseAttendancesForStudent(studentId);
            foreach (CourseAttendance attendance in attendances)
            {
                if(attendance.CourseId == courseId && _courseService.GetCourseById(courseId)!.State == Consts.CourseState.Active)
                    _courseAttendanceDAO.DeleteCourseAttendance(attendance.Id);
            }
        }

        public void GradeStudent(string studentId, string CourseId)
        {

        }

        public void RateTutor()
        {

        }

        public List<CourseAttendance> GetFinishedCoursesStudent(string studentId)
        {
            List<CourseAttendance> attendances = _courseAttendanceDAO.GeCourseAttendancesForStudent(studentId);
            foreach (CourseAttendance attendance in attendances)
            {
                Course course = _courseService.GetCourseById(attendance.CourseId)!;
                if (course.State == Consts.CourseState.Finished)
                    attendances.Add(attendance);
            }
            return attendances;
        }

    }
}
