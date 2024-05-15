using System.Collections.Generic;
using LangLang.Domain.Model;

namespace LangLang.Domain.RepositoryInterfaces;
public interface ICourseAttendanceRepository : IRepository<CourseAttendance>
{
    public List<CourseAttendance> GetForCourse(string courseId);
    public List<CourseAttendance> GetForStudent(string studentId);
    public CourseAttendance? GetStudentAttendanceForCourse(string studentId, string courseId);
}

