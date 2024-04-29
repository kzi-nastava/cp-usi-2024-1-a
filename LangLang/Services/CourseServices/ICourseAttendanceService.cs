using LangLang.Model;
using System.Collections.Generic;


namespace LangLang.Services.StudentCourseServices;
public interface ICourseAttendanceService
{
    public void RemoveAttendee(string studentId, string courseId);
    public void GradeStudent(string studentId, string CourseId);
    public void RateTutor(CourseAttendance attendance);
    public List<CourseAttendance> GetFinishedCoursesStudent(string studentId);
}

