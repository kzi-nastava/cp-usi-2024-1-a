using System.Collections.Generic;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;
using LangLang.Repositories.Json.Util;

namespace LangLang.Repositories.Json;
public class CourseApplicationDAO : ICourseApplicationDAO
{
        private readonly ILastIdDAO _lastIdDAO;

    public CourseApplicationDAO(ILastIdDAO lastIdDAO)
        {
        _lastIdDAO = lastIdDAO;
        }
    public Dictionary<string, CourseApplication> GetAllCourseApplications()
        {
            Dictionary<string, CourseApplication> courseApplications = JsonUtil.ReadFromFile<CourseApplication>(Constants.CourseApplicationsFilePath);
            return courseApplications;
        }
    public CourseApplication? GetCourseApplicationById(string id)
    {
        Dictionary<string, CourseApplication> courseApplications = GetAllCourseApplications();
        return courseApplications.GetValueOrDefault(id);
    }
    public List<CourseApplication> GetCourseApplicationsForCourse(string courseId)
    {
        List<CourseApplication> applications = new();
        foreach(CourseApplication application in GetAllCourseApplications().Values)
        {
            if(application.CourseId == courseId)
            {
                applications.Add(application);
            }
        }
        return applications;
    }
    public List<CourseApplication> GetCourseApplicationsForStudent(string studentId)
    {
        List<CourseApplication> applications = new();
        foreach(CourseApplication application in GetAllCourseApplications().Values)
        {
            if(application.StudentId == studentId)
            {
                applications.Add(application);
            }
        }
        return applications;
            }
    public CourseApplication? GetStudentApplicationForCourse(string studentId, string courseId)
    {
        List<CourseApplication> applications = GetCourseApplicationsForStudent(studentId);
        foreach(CourseApplication application in applications)
        {
            if(application.CourseId == courseId)
            {
                return application;
            }
        }
        return null;
            }
    public CourseApplication AddCourseApplication(CourseApplication application)
            {
        _lastIdDAO.IncrementCourseApplicationId();
        application.Id = _lastIdDAO.GetCourseApplicationId();

        Dictionary<string, CourseApplication> courseApplications = GetAllCourseApplications();
        courseApplications.Add(application.Id, application);
        SaveCourseApplications(courseApplications);
        return application;
            }
    public void DeleteCourseApplication(string id)
    {
        Dictionary<string, CourseApplication> courseApplications = GetAllCourseApplications();
        courseApplications.Remove(id);
        SaveCourseApplications(courseApplications);
        }
    public CourseApplication? UpdateCourseApplication(string id, CourseApplication application)
        {
        Dictionary<string, CourseApplication> courseApplications = GetAllCourseApplications();
        if (!courseApplications.ContainsKey(id)) return null;
        courseApplications[id] = application;
        return application;
        }
    private void SaveCourseApplications(Dictionary<string, CourseApplication> courseApplications)
    {
        JsonUtil.WriteToFile(courseApplications, Constants.CourseApplicationsFilePath);
    }

}

