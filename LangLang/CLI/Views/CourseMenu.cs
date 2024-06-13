using System;
using System.Collections.Generic;
using LangLang.Application.DTO;
using LangLang.Application.Stores;
using LangLang.Application.UseCases.Course;
using LangLang.Application.UseCases.Exam;
using LangLang.CLI.Util;
using LangLang.Domain.Model;
using MigraDoc.DocumentObjectModel;

namespace LangLang.CLI.Views;

public class CourseMenu : ICliMenu
{
    private readonly ICourseService _courseService;
    public Tutor? loggedInTutor;

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
            Console.WriteLine("4) For exit");
            choice = InputHandler.ReadString("Enter your choice: ");
            switch (choice)
            {
                case "1":
                    AddCourse();
                    break;
                case "2":
                    UpdateCourse();
                    break;
                case "3":
                    DeleteCourse();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid option try again.");
                    break;
            }
        }

    }

    private void UpdateCourse()
    {
        string? courseId = InputHandler.ReadString("Enter course id: ");
        if (courseId.IsValueNullOrEmpty())
        {
            Console.WriteLine("Invalid input");
            return;
        }

        if (!IsTutorsCourse(courseId!))
        {
            Console.WriteLine("Cannot update other tutors courses!");
            return;
        }

        if (loggedInTutor == null)
        {
            Console.WriteLine("User not logged in correctly.");
            return;
        }

        var course = new Form<Course>().CreateObject();
        course.Id = courseId!;
        course.TutorId = loggedInTutor.Id;

        _courseService.UpdateCourse(course);
        Console.WriteLine("Course updated successfully!");

    }

    private void DeleteCourse()
    {
        string? courseId = InputHandler.ReadString("Enter course id: ");
        if(courseId.IsValueNullOrEmpty())
        {
            Console.WriteLine("Invalid input");
            return;
        }

        if (!IsTutorsCourse(courseId!))
        {
            Console.WriteLine("Cannot delete other tutors courses!");
            return;
        }
        
        _courseService.DeleteCourse(courseId!);
        Console.WriteLine("Course deleted successfully!");

    }

    private bool IsTutorsCourse(string courseId)
    {
        if (loggedInTutor == null)
        {
            throw new ArgumentException("User not logged in correctly");
        }
        foreach (Course course in _courseService.GetCoursesByTutor(loggedInTutor))
        {
            if(course.Id == courseId)
            {
                return true;
            }
        }
        return false;
    }

    private void AddCourse()
    {
        if (loggedInTutor == null) {
            Console.WriteLine("User not logged in correctly.");
            return;
        }
        var course = new Form<Course>().CreateObject();
        course.TutorId = loggedInTutor.Id;
        _courseService.AddCourse(course);
    }

    private List<Course> GetCourses()
    {
        if (loggedInTutor == null)
        {
            throw new ArgumentException("User not logged in correctly.");
        }
        return _courseService.GetCoursesByTutor(loggedInTutor);
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