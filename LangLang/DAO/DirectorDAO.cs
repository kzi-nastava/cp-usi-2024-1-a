using Consts;
using System.Collections.Generic;
using LangLang.Model;
using LangLang.Util;
using System.Windows.Navigation;
using System.IO;

namespace LangLang.DAO
{
    public class DirectorDAO
    {
        private static DirectorDAO? instance;
        private Dictionary<string, Director>? directors;
        
        private Dictionary<string, Director> Directors
        {
            get
            {
                if (directors == null)
                    Load();
                return directors!;
            }
            set => directors = value;
        }

        private DirectorDAO()
        {
        }

        public static DirectorDAO GetInstance()
        {
            return instance ??= new DirectorDAO();
        }

        public Dictionary<string, Director> GetAllDirectors() => Directors;

        public Director? GetDirector(string email)
        {
            return Directors.GetValueOrDefault(email);
        }
        public void UpdateDirector(string id, Director director)
        {
            if (Directors.ContainsKey(id))
            {
                Directors[id] = director;
                Save();
            }
        }

        private void Load()
        {
            try
            {
                directors = JsonUtil.ReadFromFile<Director>(Constants.DirectorFilePath);
            }
            catch
            {
                Directors = new();
                Save();
            }
        }

        private void Save() => JsonUtil.WriteToFile(Directors, Constants.DirectorFilePath);
    }
}