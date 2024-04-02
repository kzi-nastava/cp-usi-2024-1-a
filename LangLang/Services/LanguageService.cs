using LangLang.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LangLang.Model;

namespace LangLang.Services
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
