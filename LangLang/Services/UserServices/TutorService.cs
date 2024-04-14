using System.Collections.Generic;
using LangLang.DAO;
using LangLang.Model;

namespace LangLang.Services.UserServices
{
    public class TutorService : ITutorService
    {
        TutorDAO tutorDAO = TutorDAO.GetInstance();
        public Tutor LoggedUser { get; set; }


        //Singleton
        private static TutorService instance;
        private TutorService()
        {
        }

        public static TutorService GetInstance()
        {
            if (instance == null)
            {
                instance = new TutorService();
                /*List<Tuple<Language, LanguageLvl>> list = new();
                list.Add(new(new Language("Serbian", "RS"), LanguageLvl.B1));
                list.Add(new(new Language("English", "EN"), LanguageLvl.A2));
                list.Add(new(new Language("Russian", "RU"), LanguageLvl.C1));
                Tutor tutor = new("b@b.b", "123", "Marko", "Profesor", DateTime.Now.AddYears(-16), Gender.Male, "123456", list, new List<Course>(), new List<Exam>(), new int[5]);
                TutorDAO.GetInstance().AddTutor(tutor);*/
            }
            return instance;
        }

        public Dictionary<string, Tutor> GetAllTutors() => tutorDAO.GetAllTutors();

        public void AddTutor(Tutor tutor) => tutorDAO.AddTutor(tutor);

        public Tutor? GetTutor(string email) => tutorDAO.GetTutor(email);

        public void DeleteTutor(string email) => tutorDAO.DeleteTutor(email);

        public void UpdateTutor(Tutor tutor) => tutorDAO.UpdateTutor(tutor);
        public void UpdateTutorEmail(Tutor tutor, string newEmail) => tutorDAO.UpdateTutorEmail(tutor, newEmail);
    }
}