using Consts;
using System.Collections.Generic;
using LangLang.Model;
using LangLang.Util;

namespace LangLang.DAO
{
    public class TutorDAO
    {
        Dictionary<string, Tutor> tutors = new();

        //Singleton
        private static TutorDAO? instance;
        private TutorDAO()
        {
        }

        public static TutorDAO GetInstance()
        {
            if (instance == null)
            {
                instance = new TutorDAO();
                instance.GetAllTutors();
            }
            return instance;
        }

        public Dictionary<string, Tutor> GetAllTutors()
        {
            if (tutors == null)
            {
                tutors = JsonUtil.ReadFromFile<Tutor>(Constants.TutorFilePath);
            }
            return tutors;
        }

        public Tutor? GetTutor(string email)
        {
            if (tutors.TryGetValue(email, out Tutor? tutor))
            {
                return tutor;
            }
            else
            {
                return null;
            }
        }

        public void AddTutor(Tutor tutor)
        {
            tutors[tutor.Email] = tutor;
            JsonUtil.WriteToFile(tutors, Constants.TutorFilePath);
        }

        public void DeleteTutor(string email) => tutors.Remove(email);

    }
}