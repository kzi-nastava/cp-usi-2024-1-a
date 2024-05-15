using System.Collections.Generic;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;

namespace LangLang.Repositories.Json;

public class CourseApplicationRepository : AutoIdRepository<CourseApplication>, ICourseApplicationRepository
{
    public CourseApplicationRepository(string filepath, string lastIdFilePath) : base(filepath, lastIdFilePath)
    {
    }

    public List<CourseApplication> GetForCourse(string courseId)
    {
        List<CourseApplication> applications = new();
        foreach (CourseApplication application in GetAll().Values)
        {
            if (application.CourseId == courseId)
            {
                applications.Add(application);
            }
        }

        return applications;
    }

    public List<CourseApplication> GetForStudent(string studentId)
    {
        List<CourseApplication> applications = new();
        foreach (CourseApplication application in GetAll().Values)
        {
            if (application.StudentId == studentId)
            {
                applications.Add(application);
            }
        }

        return applications;
    }

    public CourseApplication? GetStudentApplicationForCourse(string studentId, string courseId)
    {
        List<CourseApplication> applications = GetForStudent(studentId);
        foreach (CourseApplication application in applications)
        {
            if (application.CourseId == courseId)
            {
                return application;
            }
        }

        return null;
    }
}