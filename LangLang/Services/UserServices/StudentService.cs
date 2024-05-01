using System;
using Consts;
using LangLang.DAO;
using LangLang.Model;

namespace LangLang.Services.UserServices
{
    public class StudentService : IStudentService
    {
        private readonly IStudentDAO _studentDao;
        
        public StudentService(IStudentDAO studentDao)
        {
            _studentDao = studentDao;
        }

        //Return if the updating is successful
        public bool UpdateStudent(Student student, string password, string name, string surname, DateTime birthDate, Gender gender, string phoneNumber)
        {
            student.Name = name;
            student.Surname = surname;
            // student.Password = password;
            student.Gender = gender;
            student.BirthDate = birthDate;
            student.Gender = gender;
            student.PhoneNumber = phoneNumber;

            _studentDao.AddStudent(student);
            return true;
        }
    
        public void DeleteAccount(Student student)
        {
            _studentDao.DeleteStudent(student.Id);
        }

        public Student? GetStudentById(string studentId)
        {
            return _studentDao.GetStudent(studentId);
        }

    }

}
