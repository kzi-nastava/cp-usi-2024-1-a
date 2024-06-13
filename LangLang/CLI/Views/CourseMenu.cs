using System;
using System.Collections.Generic;
using System.Linq;
using LangLang.Application.DTO;
using LangLang.Application.UseCases.Course;
using LangLang.Application.UseCases.TutorSelection;
using LangLang.CLI.Util;
using LangLang.Domain.Model;
using MigraDoc.DocumentObjectModel;

namespace LangLang.CLI.Views;

public class CourseMenu : ICliMenu
{
    private readonly ICourseService _courseService;
    private readonly IAutoCourseTutorSelector _autoCourseTutorSelector;
    
    public Tutor? loggedInTutor;

    public CourseMenu(ICourseService courseService, IAutoCourseTutorSelector autoCourseTutorSelector)
    {
        _courseService = courseService;
        _autoCourseTutorSelector = autoCourseTutorSelector;
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

        var course = new Form<Course>().CreateObject();
        course.Id = courseId!;

        if (loggedInTutor != null)
        {
            course.TutorId = loggedInTutor.Id;
        }
        else
        {
            course.TutorId = _autoCourseTutorSelector.Select(new CourseTutorSelectionDto(course.Language, course.Level,
                course.Duration, course.Schedule, course.Schedule.Keys.ToList(), course.Start))?.Id;
        }
        
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
            return true;
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
        var course = new Form<Course>().CreateObject();
        if (loggedInTutor != null)
        {
            course.TutorId = loggedInTutor.Id;
        }
        else
        {
            course.TutorId = _autoCourseTutorSelector.Select(new CourseTutorSelectionDto(course.Language, course.Level,
                course.Duration, course.Schedule, course.Schedule.Keys.ToList(), course.Start))?.Id;
        }
        _courseService.AddCourse(course);
    }

    private List<Course> GetCourses()
    {
        if (loggedInTutor == null)
        {
            return _courseService.GetAll();
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