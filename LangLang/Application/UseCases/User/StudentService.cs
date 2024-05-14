using System;
using System.Collections.Generic;
using System.Linq;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;

namespace LangLang.Application.UseCases.User
{
    public class StudentService : IStudentService
    {
        private readonly IStudentDAO _studentDao;
        
        public StudentService(IStudentDAO studentDao)
        {
            _studentDao = studentDao;
        }

        public void UpdateStudent(Student student, string name, string surname, DateTime birthDate, Gender gender, string phoneNumber)
        {
            student.Update(name, surname, birthDate, gender, phoneNumber);  
            _studentDao.UpdateStudent(student);
        }
    
        public void DeleteAccount(Student student)
        {
            _studentDao.DeleteStudent(student.Id);
        }

        public Student? GetStudentById(string studentId)
        {
            return _studentDao.GetStudent(studentId);
        }

        public List<Student> GetAllStudents() => _studentDao.GetAllStudents().Values.ToList();

        public Student AddStudent(Student student)
        {
            return _studentDao.AddStudent(student);
        }

        public uint AddPenaltyPoint(Student student)
        {
            student.AddPenaltyPts();
            _studentDao.UpdateStudent(student);
            return student.PenaltyPts;
        }

        public void RemovePenaltyPoint(Student student)
        {
            student.RemovePenaltyPts();
            _studentDao.UpdateStudent(student);
        }
        
        public void AddLanguageSkill(Student student, Language language, LanguageLevel languageLevel)
        {
            student.AddCompletedCourseLanguage(language, languageLevel);
            _studentDao.UpdateStudent(student);
        }

        public void AddPassedLanguage(Student student, Language language, LanguageLevel languageLevel)
        {
            student.AddPassedExamLanguage(language, languageLevel);
            _studentDao.UpdateStudent(student);
        }
    }

}
