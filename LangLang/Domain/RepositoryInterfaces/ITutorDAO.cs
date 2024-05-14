using System.Collections.Generic;
using LangLang.Domain.Model;

namespace LangLang.Domain.RepositoryInterfaces;

public interface ITutorDAO
{
    public Dictionary<string, Tutor> GetAllTutors();
    public Tutor? GetTutor(string id);
    public Tutor AddTutor(Tutor tutor);
    public void UpdateTutor(Tutor tutor);
    public bool Exists(string id);
    public void DeleteTutor(string id);
}