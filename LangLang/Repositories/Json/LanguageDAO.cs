using System.Collections.Generic;
using System.IO;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;
using LangLang.Repositories.Json.Util;

namespace LangLang.Repositories.Json
{
    public class LanguageDAO : ILanguageDAO
    {
        private Dictionary<string, Language>? _languages;
        private Dictionary<string, Language> Languages
        {
            get
            {
                if(_languages == null)
                {
                    Load();
                } 
                return _languages!;
            }
            set { _languages = value; }
        }

        public Dictionary<string, Language> GetAllLanguages()
        {
            return Languages;
        }

        public void AddLanguage(Language language)
        {
           Languages[language.Name] = language;  
        }

        public void DeleteLanguage(string name)
        {
            Languages.Remove(name);
            Save();
            
        }

        public Language? GetLanguageById(string name)
        {
            try
            {
                return Languages[name];
            }catch(KeyNotFoundException)
            {
                return null;
            }
        }

        private void Load() { 
            try
            {
                _languages = JsonUtil.ReadFromFile<Language>(Constants.LanguageFilePath);
            }
            catch (DirectoryNotFoundException)
            {
                Languages = new Dictionary<string, Language>();
                Save();
            }
            catch (FileNotFoundException)
            {
                Languages = new Dictionary<string, Language>();
                Save();
            }
        }
        private void Save()
        {
            JsonUtil.WriteToFile<Language>(Languages, Constants.LanguageFilePath);
        }   
    }
}
