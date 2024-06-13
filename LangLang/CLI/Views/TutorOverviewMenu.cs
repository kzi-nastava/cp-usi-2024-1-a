using System;
using System.Collections.Generic;
using LangLang.Application.UseCases.User;
using LangLang.CLI.Util;
using LangLang.Domain.Model;

namespace LangLang.CLI.Views;

public class TutorOverviewMenu : ICliMenu
{
    private readonly ITutorService _tutorService;

    public TutorOverviewMenu(ITutorService tutorService)
    {
        _tutorService = tutorService;
    }

    public void Show()
    {
        Console.Clear();
        while (true)
        {
            Console.WriteLine("=== Tutor overview ===");
            ShowTable();
            Console.WriteLine("Select one of the following options:");
            Console.WriteLine("1) Add tutor");
            Console.WriteLine("2) Edit tutor");
            Console.WriteLine("3) Delete tutor");
            Console.WriteLine("4) Go back");
            
            var option = InputHandler.ReadInt("Enter your choice: ");
            switch (option)
            {
                case 1:
                    AddTutor();
                    break;
                case 2:
                    UpdateTutor();
                    break;
                case 3:
                    DeleteTutor();
                    break;
                case 4:
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    private List<Tutor> GetTutors()
    {
        return _tutorService.GetAllTutors();
    }

    private void ShowTable()
    {
        var tutors = GetTutors();
        if (tutors.Count == 0)
        {
            Console.WriteLine("No tutors found.");
            return;
        }

        var table = new Table<Tutor>(new TableAdapter<Tutor>(tutors));
        table.Show();
    }

    private void AddTutor()
    {
        var tutor = new Form<Tutor>().CreateObject();
        tutor.DateAdded = DateTime.Now;
        tutor.KnownLanguages = new List<Tuple<Language, LanguageLevel>>
            { new(new Language("English", "ENG"), LanguageLevel.C2) };
        _tutorService.AddTutor(tutor);
    }

    private void UpdateTutor()
    {
        var tutorId = InputHandler.ReadString("Enter tutor id: ");
        if (string.IsNullOrEmpty(tutorId))
        {
            Console.WriteLine("Invalid input");
            return;
        }
        
        var tutor = new Form<Tutor>().CreateObject();
        tutor.Id = tutorId;
        _tutorService.UpdateTutor(tutor, tutor.Name, tutor.Surname, tutor.BirthDate, tutor.Gender, tutor.PhoneNumber,
            tutor.KnownLanguages, tutor.DateAdded);
    }
    
    private void DeleteTutor()
    {
        var tutorId = InputHandler.ReadString("Enter tutor ID: ");
        if (tutorId == null)
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        var tutor = _tutorService.GetTutorById(tutorId);
        if (tutor == null)
        {
            Console.WriteLine("Tutor not found.");
            return;
        }
        
        _tutorService.DeleteAccount(tutor);
    }
}