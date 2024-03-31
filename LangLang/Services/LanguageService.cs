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
        LanguageDAO languageDAO = LanguageDAO.getInstance();

        public Dictionary<string,Language> GetAll()
        {
            return languageDAO.getAllLanguages();
        }
        public Language GetLanguageById(string name)
        {
            return languageDAO.GetLanguageById(name);
        }
    }
}
