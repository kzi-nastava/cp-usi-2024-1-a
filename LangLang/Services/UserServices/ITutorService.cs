﻿using System.Collections.Generic;
using LangLang.Model;

namespace LangLang.Services.UserServices;

public interface ITutorService
{
    public Dictionary<string, Tutor> GetAllTutors();

    public void AddTutor(Tutor tutor);

    public Tutor? GetTutor(string email);

    public Tutor? GetTutorForCourse(string courseId);

    public void AddRating(Tutor tutor, int rating);

    public void DeleteTutor(string email);

    public void UpdateTutor(Tutor tutor);
}