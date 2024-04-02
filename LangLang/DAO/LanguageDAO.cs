using Consts;
using System;
using System.Collections.Generic;
using LangLang.Model;
using LangLang.Util;
using System.IO;

namespace LangLang.DAO
{
    internal class LanguageDAO
    {
        private static LanguageDAO? instance;
        private Dictionary<string, Language>? languages;
        private Dictionary<string, Language> Languages
        {
            get
            {
                if(languages == null)
                {
                    Load();
                } 
                return languages!;
            }
            set { languages = value; }
        }

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
                languages = JsonUtil.ReadFromFile<Language>(Constants.LanguageFilePath);
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
