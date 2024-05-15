using System.Collections.Generic;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;

namespace LangLang.Repositories.Json
{
    public class CourseAttendanceRepository: AutoIdRepository<CourseAttendance>, ICourseAttendanceRepository
    {
        public CourseAttendanceRepository(string filepath, string lastIdFilePath) : base(filepath, lastIdFilePath)
        {
        }
        
        public List<CourseAttendance> GetForCourse(string courseId)
        {
            List<CourseAttendance> attendances = new();
            foreach (CourseAttendance attendance in GetAll().Values)
            {
                if (attendance.CourseId == courseId)
                {
                    attendances.Add(attendance);
                }
            }
            return attendances;
        }
        public List<CourseAttendance> GetForStudent(string studentId)
        {
            List<CourseAttendance> attendances = new();
            foreach (CourseAttendance attendance in GetAll().Values)
            {
                if (attendance.StudentId == studentId)
                {
                    attendances.Add(attendance);
                }
            }
            return attendances;
        }
        public CourseAttendance? GetStudentAttendanceForCourse(string studentId, string courseId)
        {
            List<CourseAttendance> attendances = GetForStudent(studentId);
            foreach (CourseAttendance attendance in attendances)
            {
                if (attendance.CourseId == courseId)
                {
                    return attendance;
                }
            }
            return null;
        }
    }
}
