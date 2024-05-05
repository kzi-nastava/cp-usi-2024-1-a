using LangLang.Model;
using System.Collections.Generic;
using static LangLang.Model.CourseApplication;

namespace LangLang.Services.CourseServices;

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
