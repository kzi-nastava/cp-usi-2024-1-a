using System.Collections.Generic;
using System.Linq;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;

namespace LangLang.Repositories.SQL
{
    public class LanguageRepositorySQL: ILanguageRepository
    {
        
        private readonly ApplicationDbContext _dbContext;

        public LanguageRepositorySQL(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Language> GetAll()
        {
            return _dbContext.Languages.ToList();
        }

        public Language? Get(string id)
        {
            return _dbContext.Languages.Find(id);
        }

        public List<Language> Get(List<string> ids)
        {
            return _dbContext.Languages.Where(language => ids.Contains(language.Name)).ToList();
        }

        public Language Add(Language language)
        {
            var existingLanguage = _dbContext.Languages.Find(language.Name);
            if (existingLanguage != null)
            {
                Update(language.Name, language);
            }
            else
            {
                _dbContext.Languages.Add(language);
                _dbContext.SaveChanges();
            }
            return language;
        }

        public Language? Update(string id, Language language)
        {
            var existingLanguage = _dbContext.Languages.Find(id);
            if (existingLanguage != null)
            {
                existingLanguage.Name = language.Name;
                existingLanguage.Code = language.Code;
                _dbContext.SaveChanges();
            }
            return existingLanguage;
        }

        public void Delete(string id)
        {
            var languageToDelete = _dbContext.Languages.Find(id);
            if (languageToDelete != null)
            {
                _dbContext.Languages.Remove(languageToDelete);
                _dbContext.SaveChanges();
            }
        }
        
    }
}
