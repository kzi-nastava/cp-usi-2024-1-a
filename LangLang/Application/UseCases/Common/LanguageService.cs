using System.Collections.Generic;
using System.Linq;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;

namespace LangLang.Application.UseCases.Common
{
    public class LanguageService : ILanguageService
    {
        private readonly ILanguageRepository _languageRepository;

        public LanguageService(ILanguageRepository languageRepository)
        {
            _languageRepository = languageRepository;
        }

        public List<Language> GetAll()
        {
            return _languageRepository.GetAll();
        }
        public Language? GetLanguageById(string name)
        {
            return _languageRepository.Get(name);
        }
    }
}
