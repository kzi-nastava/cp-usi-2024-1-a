using System;
using System.Collections.Generic;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;

namespace LangLang.Application.UseCases.User
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        
        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public void UpdateStudent(Student student, string name, string surname, DateTime birthDate, Gender gender, string phoneNumber)
        {
            student.Update(name, surname, birthDate, gender, phoneNumber);  
            _studentRepository.Update(student.Id, student);
        }
    
        public void DeleteAccount(Student student)
        {
            _studentRepository.Delete(student.Id);
        }

        public Student? GetStudentById(string studentId)
        {
            return _studentRepository.Get(studentId);
        }

        public List<Student> GetAllStudents() => _studentRepository.GetAll();

        public Student AddStudent(Student student)
        {
            return _studentRepository.Add(student);
        }

        public uint AddPenaltyPoint(Student student)
        {
            student.AddPenaltyPoint();
            _studentRepository.Update(student.Id, student);
            return student.PenaltyPoints;
        }

        public void RemovePenaltyPoint(Student student)
        {
            student.RemovePenaltyPoint();
            _studentRepository.Update(student.Id, student);
        }
        
        public void AddLanguageSkill(Student student, Language language, LanguageLevel languageLevel)
        {
            student.AddCompletedCourseLanguage(language, languageLevel);
            _studentRepository.Update(student.Id, student);
        }

        public void AddPassedLanguage(Student student, Language language, LanguageLevel languageLevel)
        {
            student.AddPassedExamLanguage(language, languageLevel);
            _studentRepository.Update(student.Id, student);
        }
    }

}
