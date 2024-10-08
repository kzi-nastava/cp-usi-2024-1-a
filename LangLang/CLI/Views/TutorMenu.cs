﻿using System;
using LangLang.CLI.Util;
using LangLang.Domain.Model;

namespace LangLang.CLI.Views;

public class TutorMenu : ICliMenu
{
    private readonly ExamMenu _examMenu;
    private readonly CourseMenu _courseMenu;
    
    public Tutor? LoggedInTutor { get; set; }

    public TutorMenu(ExamMenu examMenu, CourseMenu courseMenu)
    {
        _examMenu = examMenu;
        _courseMenu = courseMenu;

    }

    public void Show()
    {
        Console.Clear();
        while (true)
        {
            Console.WriteLine("=== Tutor menu ===");
            Console.WriteLine("Select one of the following options:");
            Console.WriteLine("1) Course management");
            Console.WriteLine("2) Exam management");
            Console.WriteLine("3) Exit");

            var option = InputHandler.ReadInt("Enter your choice: ");
            switch (option)
            {
                case 1:
                    _courseMenu.loggedInTutor = LoggedInTutor;
                    _courseMenu.Show();
                    Console.Clear();
                    break;
                case 2:
                    _examMenu.LoggedInTutor = LoggedInTutor;
                    _examMenu.Show();
                    Console.Clear();
                    break;
                case 3:
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }
}