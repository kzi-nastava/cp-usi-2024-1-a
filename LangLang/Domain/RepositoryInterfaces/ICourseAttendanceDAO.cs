using System.Collections.Generic;
using LangLang.Domain.Model;

namespace LangLang.Domain.RepositoryInterfaces;
public interface ICourseAttendanceDAO
{
    public Dictionary<string, CourseAttendance> GetAllCourseAttendances();
    public CourseAttendance? GetCourseAttendanceById(string id);
    public List<CourseAttendance> GetCourseAttendancesForCourse(string courseId);
    public List<CourseAttendance> GetCourseAttendancesForStudent(string studentId);
    public CourseAttendance? GetStudentAttendanceForCourse(string studentId, string courseId);
    public CourseAttendance AddCourseAttendance(CourseAttendance attendance);
    public CourseAttendance? UpdateCourseAttendance(string id, CourseAttendance attendance);
    public void DeleteCourseAttendance(string id);


}

