using LangLang.Model;
using System.Collections.Generic;
using System;

namespace LangLang.DAO;
public interface ICourseApplicationDAO
{
    public Dictionary<string, CourseApplication> GetAllCourseApplications();
    public CourseApplication? GetCourseApplicationById(string id);
    public List<CourseApplication> GetCourseApplicationsForStudent(string studentId);
    public List<CourseApplication> GetCourseApplicationsForCourse(string courseId);
    public CourseApplication? GetStudentApplicationForCourse(string studentId, string courseId);
    public CourseApplication AddCourseApplication(CourseApplication application);
    public CourseApplication? UpdateCourseApplication(string id, CourseApplication application);
    public void DeleteCourseApplication(string id);

}

