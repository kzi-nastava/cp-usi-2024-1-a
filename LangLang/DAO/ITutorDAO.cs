﻿using System.Collections.Generic;
using LangLang.Model;

namespace LangLang.DAO;

public interface ITutorDAO
{
    public Dictionary<string, Tutor> GetAllTutors();
    public Tutor? GetTutor(string email);
    public Tutor AddTutor(Tutor tutor);
    public void UpdateTutor(Tutor tutor);
    public bool Exists(string email);
    public void DeleteTutor(string email);
}