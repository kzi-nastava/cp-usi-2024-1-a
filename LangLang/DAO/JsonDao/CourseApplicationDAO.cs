using Consts;
using LangLang.Model;
using LangLang.Util;
using System.Collections.Generic;

namespace LangLang.DAO.JsonDao;
public class CourseApplicationDAO : ICourseApplicationDAO
{
        private Dictionary<string, CourseApplication>? _courseApplications;
        private readonly ILastIdDAO _lastIdDAO;

        private Dictionary<string, CourseApplication> CourseApplications
        {
            get
            {
            _courseApplications ??= JsonUtil.ReadFromFile<CourseApplication>(Constants.CourseApplicationsFilePath);
            return _courseApplications;
                }
        set => _courseApplications = value;
            
        }

    public CourseApplicationDAO(ILastIdDAO lastIdDAO)
        {
        _lastIdDAO = lastIdDAO;
        }
    public Dictionary<string, CourseApplication> GetAllCourseApplications()
        {
            return CourseApplications;
        }
    public CourseApplication? GetCourseApplicationById(string id)
    {
        return CourseApplications.GetValueOrDefault(id);
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
        CourseApplications.Add(application.Id, application);
        SaveCourseApplications();
        return application;
            }
    public void DeleteCourseApplication(string id)
    {
        CourseApplications.Remove(id);
        SaveCourseApplications();
        }
    public CourseApplication? UpdateCourseApplication(string id, CourseApplication application)
        {
        if (!CourseApplications.ContainsKey(id)) return null;
        CourseApplications[id] = application;
        return application;
        }
    private void SaveCourseApplications()
    {
        JsonUtil.WriteToFile(CourseApplications, Constants.CourseApplicationsFilePath);
    }

}

