using System.Collections.Generic;
using LangLang.Domain.Model;

namespace LangLang.Application.UseCases.Course;
public interface ICourseAttendanceService
{
    public List<CourseAttendance> GetAttendancesForStudent(string studentId);
    public List<CourseAttendance> GetAttendancesForCourse(string courseId);
    public CourseAttendance? GetStudentAttendance(string studentId);
    public List<CourseAttendance> GetFinishedCoursesStudent(string studentId);
    public CourseAttendance AddAttendance(string studentId, string courseId);
    public void RemoveAttendee(string studentId, string courseId);
    public bool RateTutor(string courseId, string studentId, int rating);
    public CourseAttendance? GetAttendance(string studentId, string courseId);
    public CourseAttendance? GradeStudent(string studentId, string CourseId, int knowledgeGrade, int activityGrade);

}

