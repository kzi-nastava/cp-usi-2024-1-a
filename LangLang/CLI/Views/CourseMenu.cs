using System;
using System.Collections.Generic;
using LangLang.Application.DTO;
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
        Console.Clear();
        string? choice;
        while (true)
        {
            Console.WriteLine("=== Course ===");
            ShowTable();
            Console.WriteLine("1) Add course");
            Console.WriteLine("2) Update course");
            Console.WriteLine("3) Delete course");
            Console.WriteLine("X) For exit");
            choice = InputHandler.ReadString();
            switch (choice)
            {
                case "1":
                    AddCourse();
                    break;
                case "2":

                    break;
                case "3":

                    break;
                case "x":
                    return;
                default:
                    Console.WriteLine("Invalid option try again.");
                    break;
            }
        }

    }
    private void AddCourse()
    {
        var course = new Form<Course>().CreateObject();
        _courseService.AddCourse(course);
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