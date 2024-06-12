using System;
using System.Collections.Generic;
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
        ShowTable();
        Console.ReadLine();
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