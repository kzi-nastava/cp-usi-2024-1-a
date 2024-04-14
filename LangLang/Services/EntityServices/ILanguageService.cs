using System.Collections.Generic;
using LangLang.Model;

namespace LangLang.Services.EntityServices;

public interface ILanguageService
{
    public List<Language> GetAll();

    public Language? GetLanguageById(string name);
}