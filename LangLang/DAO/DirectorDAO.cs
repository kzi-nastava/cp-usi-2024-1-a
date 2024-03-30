using Consts;
using System.Collections.Generic;
using LangLang.Model;
using LangLang.Util;

namespace LangLang.DAO
{
    public class DirectorDAO
    {
        Dictionary<string, Director> directors = new();

        //Singleton
        private static DirectorDAO? instance;
        private DirectorDAO()
        {
        }

        public static DirectorDAO GetInstance()
        {
            if (instance == null)
            {
                instance = new DirectorDAO();
                instance.GetAllDirectors();
            }
            return instance;
        }

        public Dictionary<string, Director> GetAllDirectors()
        {
            if (directors == null)
            {
                directors = JsonUtil.ReadFromFile<Director>(Constants.DirectorFilePath);
            }
            return directors;
        }

        public Director? GetDirector(string email)
        {
            if (directors.TryGetValue(email, out Director? director))
            {
                return director;
            }
            else
            {
                return null;
            }
        }

        public void AddDirector(Director director)
        {
            directors[director.Email] = director;
            JsonUtil.WriteToFile(directors, Constants.DirectorFilePath);
        }

        public void DeleteDirector(string email) => directors.Remove(email);

    }
}