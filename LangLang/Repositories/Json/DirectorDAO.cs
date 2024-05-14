using System.Collections.Generic;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;
using LangLang.Repositories.Json.Util;

namespace LangLang.Repositories.Json
{
    public class DirectorDAO : IDirectorDAO
    {
        private Dictionary<string, Director>? _directors;
        
        private Dictionary<string, Director> Directors
        {
            get
            {
                if (_directors == null)
                    Load();
                return _directors!;
            }
            set => _directors = value;
        }

        public Dictionary<string, Director> GetAllDirectors() => Directors;

        public Director? GetDirector(string id)
        {
            return Directors.GetValueOrDefault(id);
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
                _directors = JsonUtil.ReadFromFile<Director>(Constants.DirectorFilePath);
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