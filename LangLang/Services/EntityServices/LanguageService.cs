using System.Collections.Generic;
using System.Linq;
using LangLang.DAO;
using LangLang.Model;

namespace LangLang.Services.EntityServices
{
    public class LanguageService : ILanguageService
    {
        private readonly ILanguageDAO _languageDao;

        public LanguageService(ILanguageDAO languageDao)
        {
            _languageDao = languageDao;
        }

        public List<Language> GetAll()
        {
            return _languageDao.GetAllLanguages().Values.ToList();
        }
        public Language? GetLanguageById(string name)
        {
            return _languageDao.GetLanguageById(name);
        }
    }
}
