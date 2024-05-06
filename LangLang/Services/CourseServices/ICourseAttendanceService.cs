using LangLang.Model;
using System.Collections.Generic;


namespace LangLang.Services.CourseServices;
public interface ICourseAttendanceService
{
    public List<CourseAttendance> GetAttendancesForStudent(string studentId);
    public List<CourseAttendance> GetAttendancesForCourse(string courseId);
    public CourseAttendance? GetStudentAttendance(string studentId);
    public List<CourseAttendance> GetFinishedCoursesStudent(string studentId);
    public CourseAttendance AddAttendance(string studentId, string courseId);
    public void RemoveAttendee(string studentId, string courseId);
    public void GradeStudent(string studentId, string CourseId, int knowledgeGrade, int activityGrade);
    public void RateTutor(CourseAttendance attendance, int rating);

}

