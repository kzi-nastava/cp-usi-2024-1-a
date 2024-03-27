using Consts;
using System;
using System.Collections.Generic;
using LangLang.Model;
using LangLang.Util;

namespace LangLang.DAO
{
    internal class LanguageDAO
    {
        private static LanguageDAO? instance;
        private Dictionary<string, Language>? languages;

        private LanguageDAO()
        {

        }

        public static LanguageDAO getInstance()
        {
            if(instance == null)
            {
                instance = new LanguageDAO();
            }
            return instance;
        }

        public Dictionary<string, Language> getAllLanguages()
        {
            if(languages == null)
            {
                languages = JsonUtil.ReadFromFile<Language>(Constants.LanguageFilePath);
            }
            return languages;
        }

        public void AddLanguage(Language language)
        {
            if(languages != null)
            {
            languages[language.Name] = language;
        }
        }

        public void DeleteLanguage(string name)
        {
            if(languages != null)
            {
            languages.Remove(name);
        }
        }

        public Language GetLanguageById(string name)
        {
            if(languages == null)
            {
                languages = new Dictionary<string, Language>();
            }
            return languages[name];
        }





    }
}
