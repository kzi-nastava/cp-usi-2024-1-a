using System.Collections.Generic;
using System.Collections.ObjectModel;
using LangLang.Domain.Model;

namespace LangLang.Application.UseCases.Course;

public interface IStudentCourseCoordinator
{
    public void Accept(string studentId, string courseId);
    public List<Domain.Model.Course> GetAvailableCourses(string studentId);
    public Domain.Model.Course? GetStudentAttendingCourse(string studentId);
    public List<Domain.Model.Course> GetFinishedAndGradedCourses();
    public List<Domain.Model.Course> GetFinishedCoursesStudent(string studentId);
    public List<Domain.Model.Course> GetAppliedCoursesStudent(string studentId);
    public List<Student> GetAppliedStudentsCourse(string courseId);
    public List<Student> GetAttendanceStudentsCourse(string courseId);
    public void SendNotification(string message, string receiverId);
    public void CancelApplication(string applicationId);
    public void CancelApplication(string studentId, string courseId);
    public void RemoveAttendee(string studentId);
    public void ApplyForCourse(string courseId, string studentId);
    public List<Domain.Model.Course> GetFinishedCourses();
    public void FinishCourse(string courseId, ObservableCollection<Student> students);
    public void GenerateAttendance(string courseId);
    public void DropCourse(string studentId, string message);
    public void AcceptDropRequest(Domain.Model.DropRequest dropRequest);
    public void DenyDropRequest(Domain.Model.DropRequest dropRequest);
    public bool CanDropCourse(string courseId);
    public List<CourseAttendance> GetGradedAttendancesForLastYear();
    public void RemoveCoursesOfTutor(Tutor tutor);
}
