using System;
using System.Collections.Generic;
using Consts;
using LangLang.Model;

namespace LangLang.Services.UserServices;

public interface ITutorService
{
    public Dictionary<string, Tutor> GetAllTutors();

    public Tutor AddTutor(Tutor tutor);

    public Tutor? GetTutorById(string id);

    public Tutor? GetTutorForCourse(string courseId);

    public void AddRating(Tutor tutor, int rating);

    public void DeleteAccount(Tutor tutor);

    public bool UpdateTutor(Tutor tutor, string name, string surname, DateTime birthDate, Gender gender,
        string phoneNumber, List<Tuple<Language, LanguageLvl>> knownLanguages, DateTime dateAdded);
}