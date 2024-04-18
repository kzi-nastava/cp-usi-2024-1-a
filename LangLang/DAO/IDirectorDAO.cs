using System.Collections.Generic;
using LangLang.Model;

namespace LangLang.DAO;

public interface IDirectorDAO
{
    public Dictionary<string, Director> GetAllDirectors();
    public Director? GetDirector(string email);
    public void UpdateDirector(string id, Director director);
}