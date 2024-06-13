using System;
using System.Collections.Generic;
using LangLang.Application.DTO;
using LangLang.Application.UseCases.Exam;
using LangLang.Application.UseCases.TutorSelection;
using LangLang.CLI.Util;
using LangLang.Domain.Model;

namespace LangLang.CLI.Views;

public class ExamMenu : ICliMenu
{
    private readonly IExamService _examService;
    private readonly IAutoExamTutorSelector _autoExamTutorSelector;
    
    public Tutor? LoggedInTutor { get; set; }

    public ExamMenu(IExamService examService, IAutoExamTutorSelector autoExamTutorSelector)
    {
        _examService = examService;
        _autoExamTutorSelector = autoExamTutorSelector;
    }

    public void Show()
    {
        Console.Clear();
        while (true)
        {
            Console.WriteLine("=== Exam ===");
            ShowTable();
            Console.WriteLine("1) Add exam");
            Console.WriteLine("2) Update exam");
            Console.WriteLine("3) Delete exam");
            Console.WriteLine("4) For exit");
            var choice = InputHandler.ReadString("Enter your choice: ");
            switch (choice)
            {
                case "1":
                    AddExam();
                    break;
                case "2":
                    UpdateExam();
                    break;
                case "3":
                    DeleteExam();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid option. Try again.");
                    break;
            }
        }
    }
    private void AddExam()
    {
        var examDto = new Form<ExamDto>().CreateObject();
        if (LoggedInTutor != null)
        {
            examDto.Tutor = LoggedInTutor;
        }
        else
        {
            examDto.Tutor = _autoExamTutorSelector.Select(new ExamTutorSelectionDto(examDto.Language!,
                examDto.LanguageLevel!.Value, examDto.Date!.Value, examDto.Time!.Value));
        }
        try
        {
            var exam = _examService.AddExam(examDto);
            Console.WriteLine($"Exam with ID {exam.Id} added successfully.");
        } catch(ArgumentException e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private void UpdateExam()
    {
        var examId = InputHandler.ReadString("Enter exam id: ");
        if (examId == null || _examService.GetExamById(examId) == null)
        {
            Console.WriteLine("Invalid exam id.");
        }
        
        var examDto = new Form<ExamDto>().CreateObject();
        examDto.Id = examId;

        try
        {
            var exam = _examService.UpdateExam(examDto);
            Console.WriteLine($"Exam with ID {exam.Id} updated successfully.");
        } catch(ArgumentException e)
        {
            Console.WriteLine(e.Message);
        }
    }
    
    private void DeleteExam()
    {
        var examId = InputHandler.ReadString("Enter the ID of the exam you want to delete: ");
        if (examId == null)
        {
            Console.WriteLine("Invalid ID.");
            return;
        }
        
        _examService.DeleteExam(examId);
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