using System;
using System.Collections.Generic;
using System.Linq;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;

namespace LangLang.Application.UseCases.User
{
    public class TutorService : ITutorService
    {
        private readonly ITutorRepository _tutorRepository;

        public TutorService(ITutorRepository tutorRepository)
        {
            _tutorRepository = tutorRepository;
        }

        public List<Tutor> GetAllTutors() => _tutorRepository.GetAll();

        public void AddRating(Tutor tutor, int rating)
        {
            tutor.AddRating(rating);
            _tutorRepository.Update(tutor.Id, tutor);
        }

        public Tutor AddTutor(Tutor tutor) => _tutorRepository.Add(tutor);

        public Tutor? GetTutorById(string id) => _tutorRepository.Get(id);

        public void DeleteAccount(Tutor tutor) => _tutorRepository.Delete(tutor.Id);

        public bool UpdateTutor(Tutor tutor, string name, string surname, DateTime birthDate, Gender gender, string phoneNumber, List<Tuple<Language, LanguageLevel>> knownLanguages, DateTime dateAdded)
        {
            tutor.Name = name;
            tutor.Surname = surname;
            tutor.Gender = gender;
            tutor.BirthDate = birthDate;
            tutor.Gender = gender;
            tutor.PhoneNumber = phoneNumber;
            tutor.KnownLanguages = knownLanguages;
            tutor.DateAdded = dateAdded;

            _tutorRepository.Update(tutor.Id, tutor);
            return true;
        }

        public List<Tutor> GetFilteredTutors(string? languageName = null, LanguageLevel? languageLevel = null,
                                  DateTime? dateAddedMin = null, DateTime? dateAddedMax = null) =>
            GetAllTutors().Where(tutor => tutor.MatchesFilter(languageName, languageLevel, dateAddedMin, dateAddedMax)).ToList();
    }
}