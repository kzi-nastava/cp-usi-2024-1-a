using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;

namespace LangLang.Repositories.Json
{
    public class LanguageRepository : Repository<Language>, ILanguageRepository
    {
        public LanguageRepository(string filepath) : base(filepath)
        {
        }

        protected override string GetId(Language language)
        {
            return language.Name;
        }
    }
}
