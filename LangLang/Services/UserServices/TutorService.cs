using System.Collections.Generic;
using LangLang.DAO;
using LangLang.Model;

namespace LangLang.Services.UserServices
{
    public class TutorService : ITutorService
    {
        TutorDAO tutorDAO = TutorDAO.GetInstance();
        public Tutor LoggedUser { get; set; }

        public Dictionary<string, Tutor> GetAllTutors() => tutorDAO.GetAllTutors();

        public void AddTutor(Tutor tutor) => tutorDAO.AddTutor(tutor);

        public Tutor? GetTutor(string email) => tutorDAO.GetTutor(email);

        public void DeleteTutor(string email) => tutorDAO.DeleteTutor(email);

        public void UpdateTutor(Tutor tutor) => tutorDAO.UpdateTutor(tutor);
        public void UpdateTutorEmail(Tutor tutor, string newEmail) => tutorDAO.UpdateTutorEmail(tutor, newEmail);
    }
}