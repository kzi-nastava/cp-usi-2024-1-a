using System.Collections.Generic;
using LangLang.Domain.Model;
using static LangLang.Domain.Model.CourseApplication;

namespace LangLang.Application.UseCases.Course;

public interface ICourseApplicationService
{
    public void RemoveStudentApplications(string studentId);
    public CourseApplication ApplyForCourse(string studentId, string courseId);
    public CourseApplication? GetApplication(string studentId, string courseId);
    public List<CourseApplication> GetApplicationsForStudent(string studentId);
    public List<CourseApplication> GetApplicationsForCourse(string courseId);
    public List<CourseApplication> GetPendingApplicationsForCourse(string courseId);
    public CourseApplication? GetCourseApplicationById(string id);
    public CourseApplication ChangeApplicationState(string applicationId, State state);
    public void RejectApplication(string applicationId);
    public void ActivateStudentApplications(string studentId);
    public void PauseStudentApplications(string studentId);
    public void DeleteApplication(string applicationId);
    public void CancelApplication(string applicationId);

}
