using LangLang.Model;
using System.Collections.Generic;

namespace LangLang.Services.CourseServices;

public interface IStudentCourseCoordinator
{
    public void Accept(string applicationId);
    public Course? GetStudentAttendingCourse(string studentId);
    public List<Course> GetFinishedCoursesStudent(string studentId);
    public List<Course> GetAppliedCoursesStudent(string studentId);
    public void CancelApplication(string applicationId);
    public void CancelApplication(string studentId, string courseId);
    public void RemoveAttendee(string studentId);
    public void ApplyForCourse(string courseId, string studentId);
    public void FinishCourse(string courseId, string studentId);
    public void GenerateAttendance(string courseId);
    public void DropCourse(string courseId);

}
