using System.Collections.Generic;
using LangLang.Domain.Model;

namespace LangLang.Domain.RepositoryInterfaces;

public interface IDirectorDAO
{
    public Dictionary<string, Director> GetAllDirectors();
    public Director? GetDirector(string id);
    public void UpdateDirector(string id, Director director);
}