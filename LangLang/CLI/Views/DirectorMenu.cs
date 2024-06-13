using System;
using LangLang.CLI.Util;

namespace LangLang.CLI.Views;

public class DirectorMenu : ICliMenu
{
    private readonly TutorOverviewMenu _tutorOverviewMenu;
    private readonly ExamMenu _examMenu;
    private readonly CourseMenu _courseMenu;
    
    public DirectorMenu(TutorOverviewMenu tutorOverviewMenu, ExamMenu examMenu, CourseMenu courseMenu)
    {
        _tutorOverviewMenu = tutorOverviewMenu;
        _examMenu = examMenu;
        _courseMenu = courseMenu;
    }

    public void Show()
    {
        Console.Clear();
        while (true)
        {
            Console.WriteLine("=== Director menu ===");
            Console.WriteLine("Select one of the following options:");
            Console.WriteLine("1) Tutor management");
            Console.WriteLine("2) Course management");
            Console.WriteLine("3) Exam management");
            Console.WriteLine("4) Exit");

            var option = InputHandler.ReadInt("Enter your choice: ");
            switch (option)
            {
                case 1:
                    _tutorOverviewMenu.Show();
                    Console.Clear();
                    break;
                case 2:
                    _courseMenu.Show();
                    Console.Clear();
                    break;
                case 3:
                    _examMenu.Show();
                    Console.Clear();
                    break;
                case 4:
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }
}