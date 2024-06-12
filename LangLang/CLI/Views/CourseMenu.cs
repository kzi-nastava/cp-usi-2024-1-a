using System;
using System.Collections.Generic;
using LangLang.Application.UseCases.Course;
using LangLang.Application.UseCases.Exam;
using LangLang.CLI.Util;
using LangLang.Domain.Model;

namespace LangLang.CLI.Views;

public class CourseMenu : ICliMenu
{
    private readonly ICourseService _courseService;

    public CourseMenu(ICourseService courseService)
    {
        _courseService = courseService;
    }

    public void Show()
    {
        ShowTable();
        Console.ReadLine();
    }

    private List<Course> GetCourses()
    {
        return _courseService.GetAll();
    }

    private void ShowTable()
    {
        var courses = GetCourses();
        if (courses.Count == 0)
        {
            Console.WriteLine("No courses found.");
            return;
        }

        var table = new Table<Course>(new TableAdapter<Course>(courses));
        table.Show();
    }
}