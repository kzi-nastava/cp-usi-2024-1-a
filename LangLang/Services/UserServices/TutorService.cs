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

        public Dictionary<string, Tutor> GetAllTutors() => _tutorDao.GetAllTutors();

        public Tutor? GetTutorForCourse(string courseId)
        {
            Dictionary<string, Tutor> allTutors = _tutorDao.GetAllTutors();
            foreach(Tutor tutor in allTutors.Values)
            {
                List<string> courses = tutor.Courses;
                foreach(string teachingCourseId in courses)
                {
                    if (teachingCourseId == courseId) return tutor;
                }
            }
            return null;
        }

        public void AddRating(Tutor tutor, int rating)
        {
            tutor.AddRating(rating);
        }

        public void AddTutor(Tutor tutor) => _tutorDao.AddTutor(tutor);

        public Tutor? GetTutor(string email) => _tutorDao.GetTutor(email);

        public void DeleteTutor(string email) => _tutorDao.DeleteTutor(email);

        public void UpdateTutor(Tutor tutor) => _tutorDao.UpdateTutor(tutor);
    }
}