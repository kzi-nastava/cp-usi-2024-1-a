using System.Collections.Generic;
using LangLang.Domain.Model;

namespace LangLang.Application.UseCases.Common;

public interface ILanguageService
{
    public List<Language> GetAll();

    public Language? GetLanguageById(string name);
}