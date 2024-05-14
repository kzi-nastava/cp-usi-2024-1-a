using System;
using System.Collections.Generic;
using LangLang.Domain.Model;

namespace LangLang.Domain.RepositoryInterfaces;

public interface ICourseDAO
{
    public Dictionary<string, Course> GetAllCourses();
    public Course? GetCourseById(string id);
    public List<Course> GetCoursesByDate(DateOnly date);
    public void AddCourse(Course course);
    public void DeleteCourse(string id);
    public void UpdateCourse(Course course);
}