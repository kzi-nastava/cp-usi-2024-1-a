using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using Consts;
using LangLang.DAO;
using LangLang.Model;

namespace LangLang.Services.AuthenticationServices
{
    public class RegisterService : IRegisterService
    {
        private readonly IStudentDAO _studentDao;
        private readonly ITutorDAO _tutorDao;
        private readonly IDirectorDAO _directorDao;

        public RegisterService(IStudentDAO studentDao, ITutorDAO tutorDao, IDirectorDAO directorDao)
        {
            _studentDao = studentDao;
            _tutorDao = tutorDao;
            _directorDao = directorDao;
        }

        public bool RegisterStudent(string email, string password, string name, string surname, DateTime birthDay, Gender gender, string phoneNumber, EducationLvl educationLvl)
        {
            bool passed = CheckUserData(email, password, name, surname, phoneNumber);
            passed &= !(CheckExistingEmail(email));
            passed &= (birthDay != DateTime.MinValue);

            if (passed)
            {
                _studentDao.AddStudent(new Student(email, password, name, surname, birthDay, gender, phoneNumber, educationLvl, 0, "", "", finishedCourses: new List<string>(),coursesApplied: new List<string>(), examsApplied: new List<string>(), notifications: new List<string>()));
            }
            return passed;
        }

        public bool CheckExistingEmail(string email)
        {
            if(email == null)
            {
                return false;
            }
            
            if (_studentDao.GetStudent(email) != null)  //or other searches
                return true;
            if (_tutorDao.GetTutor(email) != null)  //or other searches
                return true;
            if (_directorDao.GetDirector(email) != null)  //or other searches
                return true;
            return false;
        }

        public bool CheckUserData(string email, string password, string name, string surname, string phoneNumber)
        {
            if(FieldsEmpty(email, password, name, surname, phoneNumber))
            {
                return false;
            }
            bool passed = true;
            passed &= CheckName(name, surname);
            passed &= CheckPassword(password);
            passed &= CheckPhoneNumber(phoneNumber);
            try
            {
                _ = new MailAddress(email);
            }
            catch
            {
                passed = false;
            }
            return passed;
        }

        private bool FieldsEmpty(string email, string password, string name, string surname, string phoneNumber)
        {
            return email == null || password == null || name == null || surname == null || phoneNumber == null;
        }


        public bool CheckPassword(string password)
        {
            return password.Length >= 8 && password.Any(char.IsDigit) && password.Any(char.IsUpper);
        }

        public bool CheckPhoneNumber(string phoneNumber)
        {
            foreach (char c in phoneNumber)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return phoneNumber.Length >= 6;
        }

        private static bool CheckName(string name, string surname)
        {
            return !name.Any(char.IsDigit) && !surname.Any(char.IsDigit);
        }
    }
}
