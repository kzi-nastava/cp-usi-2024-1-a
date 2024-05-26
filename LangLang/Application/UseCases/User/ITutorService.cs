using System;
using System.Collections.Generic;
using LangLang.Domain.Model;

namespace LangLang.Application.UseCases.User;

public interface ITutorService
{
    public List<Tutor> GetAllTutors();

    public Tutor AddTutor(Tutor tutor);
    
    public Tutor? GetTutorById(string id);
    
    public void AddRating(Tutor tutor, int rating);

    public void DeleteAccount(Tutor tutor);

    public bool UpdateTutor(Tutor tutor, string name, string surname, DateTime birthDate, Gender gender,
        string phoneNumber, List<Tuple<Language, LanguageLevel>> knownLanguages, DateTime dateAdded);

    public List<Tutor> GetFilteredTutors(string? languageName = null, LanguageLevel? languageLevel = null,
                                  DateTime? dateAddedMin = null, DateTime? dateAddedMax = null);
    public List<Tutor> GetAllTutorsForPage(int pageNumber, int tutorsPerPage);
    public List<Tutor> GetFilteredTutorsForPage(int pageNumber, int tutorsPerPage, string? languageName = null, LanguageLevel? languageLevel = null,
                                  DateTime? dateAddedMin = null, DateTime? dateAddedMax = null);

}