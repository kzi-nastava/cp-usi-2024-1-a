using System.Collections.Generic;
using LangLang.Domain.Model;

namespace LangLang.Domain.RepositoryInterfaces;

public interface ILanguageDAO
{
    public Dictionary<string, Language> GetAllLanguages();
    public void AddLanguage(Language language);
    public void DeleteLanguage(string name);
    public Language? GetLanguageById(string name);
}