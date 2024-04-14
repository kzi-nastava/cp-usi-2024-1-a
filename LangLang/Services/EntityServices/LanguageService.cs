using System.Collections.Generic;
using System.Linq;
using LangLang.DAO;
using LangLang.Model;

namespace LangLang.Services.EntityServices
{
    class LanguageService
    {
        LanguageDAO languageDAO = LanguageDAO.GetInstance();

        public List<Language> GetAll()
        {
            return languageDAO.GetAllLanguages().Values.ToList();
        }
        public Language? GetLanguageById(string name)
        {
            return languageDAO.GetLanguageById(name);
        }
    }
}
