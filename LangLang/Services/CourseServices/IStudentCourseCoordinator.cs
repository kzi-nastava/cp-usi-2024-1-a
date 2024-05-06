using LangLang.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LangLang.Services.CourseServices;

public interface IStudentCourseCoordinator
{
    public void Accept(string studentId, string courseId);
    public List<Course> GetAvailableCourses(string studentId);
    public Course? GetStudentAttendingCourse(string studentId);
    public List<Course> GetFinishedCoursesStudent(string studentId);
    public List<Course> GetAppliedCoursesStudent(string studentId);
    public List<Student> GetAppliedStudentsCourse(string courseId);
    public List<Student> GetAttendanceStudentsCourse(string courseId);
    public void SendNotification(string message, string receiverId);
    public void CancelApplication(string applicationId);
    public void CancelApplication(string studentId, string courseId);
    public void RemoveAttendee(string studentId);
    public void ApplyForCourse(string courseId, string studentId);
    public void FinishCourse(string courseId, ObservableCollection<Student> students);
    public void GenerateAttendance(string courseId);
    public void DropCourse(string studentId, string message);
    public void AcceptDropRequest(DropRequest dropRequest);
    public void DenyDropRequest(DropRequest dropRequest);

}
