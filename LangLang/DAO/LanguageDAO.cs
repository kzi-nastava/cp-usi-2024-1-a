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
        private static LanguageDAO? _instance;
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

        private LanguageDAO()
        {

        }

        public static LanguageDAO GetInstance()
        {
            if(_instance == null)
            {
                _instance = new LanguageDAO();
            }
            return _instance;
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
