using LangLang.Model;
using System.Collections.Generic;

namespace LangLang.DAO;
public interface ICourseAttendanceDAO
{
    public Dictionary<string, CourseAttendance> GetAllCourseAttendances();
    public CourseAttendance? GetCourseAttendanceById(string id);
    public List<CourseAttendance> GetCourseAttendancesForCourse(string studentId);
    public List<CourseAttendance> GeCourseAttendancesForStudent(string courseId);
    public CourseAttendance? GetStudentAttendanceForCourse(string studentId, string courseId);
    public CourseAttendance AddCourseAttendance(CourseAttendance attendance);
    public CourseAttendance? UpdateCourseAttendance(string id, CourseAttendance attendance);
    public void DeleteCourseAttendance(string id);


}

