using System.Collections.Generic;
using LangLang.Domain.Model;

namespace LangLang.Domain.RepositoryInterfaces;
public interface ICourseApplicationRepository : IRepository<CourseApplication>
{
    public List<CourseApplication> GetForStudent(string studentId);
    public List<CourseApplication> GetForCourse(string courseId);
    public CourseApplication? GetStudentApplicationForCourse(string studentId, string courseId);
}

