using System;
using System.Collections.Generic;
using System.Windows.Input;
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

        public Tutor? GetTutorForCourse(string courseId)
        {
            foreach(Tutor tutor in _tutorRepository.GetAll())
            {
                List<string> courses = tutor.Courses;
                foreach(string teachingCourseId in courses)
                {
                    if (teachingCourseId == courseId) return tutor;
                }
            }
            return null;
        }

        public string GetTutorNameForCourse(string courseId)
        {
            Tutor tutor = GetTutorForCourse(courseId)!;
            return tutor.GetFullName();
        }

        public Tutor? GetTutorForExam(string examId)
        {
            foreach (Tutor tutor in _tutorRepository.GetAll())
            {
                List<string> exams = tutor.Exams;
                foreach (string teachingExamId in exams)
                {
                    if (teachingExamId == examId) return tutor;
                }
            }
            return null;
        }

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
    }
}