using System.Collections.Generic;
using LangLang.DAO;
using LangLang.Model;

namespace LangLang.Services.UserServices
{
    public class TutorService : ITutorService
    {
        private readonly ITutorDAO _tutorDao;

        public TutorService(ITutorDAO tutorDao)
        {
            _tutorDao = tutorDao;
        }

        public Tutor LoggedUser { get; set; }

        public Dictionary<string, Tutor> GetAllTutors() => _tutorDao.GetAllTutors();

        public void AddTutor(Tutor tutor) => _tutorDao.AddTutor(tutor);

        public Tutor? GetTutor(string email) => _tutorDao.GetTutor(email);

        public void DeleteTutor(string email) => _tutorDao.DeleteTutor(email);

        public void UpdateTutor(Tutor tutor) => _tutorDao.UpdateTutor(tutor);
        public void UpdateTutorEmail(Tutor tutor, string newEmail) => _tutorDao.UpdateTutorEmail(tutor, newEmail);
    }
}