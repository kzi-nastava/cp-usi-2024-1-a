using System;
using System.Collections.Generic;
using LangLang.Application.DTO;
using LangLang.Application.UseCases.Exam;
using LangLang.CLI.Util;
using LangLang.Domain.Model;

namespace LangLang.CLI.Views;

public class ExamMenu : ICliMenu
{
    private readonly IExamService _examService;

    public ExamMenu(IExamService examService)
    {
        _examService = examService;
    }

    public void Show()
    {
        Console.Clear();
        string? choice;
        while (true)
        {
            Console.WriteLine("=== Exam ===");
            ShowTable();
            Console.WriteLine("1) Add exam");
            Console.WriteLine("2) Update exam");
            Console.WriteLine("3) Delete exam");
            Console.WriteLine("4) For exit");
            choice = InputHandler.ReadString("Enter your choice: ");
            switch (choice)
            {
                case "1":
                    AddExam();
                    break;
                case "2":

                    break;
                case "3":

                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid option try again.");
                    break;
            }
        }
    }
    private void AddExam()
    {
        var exam = new Form<ExamDto>().CreateObject();
        _examService.AddExam(exam);
    }

    private List<Exam> GetExams()
    {
        return _examService.GetAllExams();
    }

    private void ShowTable()
    {
        var exams = GetExams();
        if (exams.Count == 0)
        {
            Console.WriteLine("No exams found.");
            return;
        }
        
        var table = new Table<Exam>(new TableAdapter<Exam>(exams));
        table.Show();
    }
}