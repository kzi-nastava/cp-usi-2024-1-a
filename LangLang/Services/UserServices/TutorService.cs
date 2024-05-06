using System.Collections.Generic;
using Consts;
using System.Xml.Linq;
using LangLang.DAO;
using LangLang.DAO.JsonDao;
using LangLang.Model;
using System;

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

        public Tutor AddTutor(Tutor tutor) => _tutorDao.AddTutor(tutor);

        public Tutor? GetTutorById(string id) => _tutorDao.GetTutor(id);

        public void DeleteAccount(Tutor tutor) => _tutorDao.DeleteTutor(tutor.Id);

        public bool UpdateTutor(Tutor tutor, string name, string surname, DateTime birthDate, Gender gender, string phoneNumber, List<Tuple<Language, LanguageLvl>> knownLanguages, DateTime dateAdded)
        {
            tutor.Name = name;
            tutor.Surname = surname;
            tutor.Gender = gender;
            tutor.BirthDate = birthDate;
            tutor.Gender = gender;
            tutor.PhoneNumber = phoneNumber;
            tutor.KnownLanguages = knownLanguages;
            tutor.DateAdded = dateAdded;

            _tutorDao.UpdateTutor(tutor);
            return true;
        }
    }
}