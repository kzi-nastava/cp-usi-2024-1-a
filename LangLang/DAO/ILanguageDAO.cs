using System.Collections.Generic;
using LangLang.Model;

namespace LangLang.DAO;

public interface ILanguageDAO
{
    public Dictionary<string, Language> GetAllLanguages();
    public void AddLanguage(Language language);
    public void DeleteLanguage(string name);
    public Language? GetLanguageById(string name);
}