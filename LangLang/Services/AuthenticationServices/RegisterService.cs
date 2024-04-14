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
        public bool RegisterStudent(string email, string password, string name, string surname, DateTime birthDay, Gender gender, string phoneNumber, EducationLvl educationLvl)
        {
            StudentDAO sd = StudentDAO.GetInstance();
            bool passed = CheckUserData(email, password, name, surname, phoneNumber);
            passed &= !(CheckExistingEmail(email));
            passed &= (birthDay != DateTime.MinValue);

            if (passed)
            {
                sd.AddStudent(new Student(email, password, name, surname, birthDay, gender, phoneNumber, educationLvl, 0, "", "", coursesApplied: new List<string>(), examsApplied: new List<string>(), notifications: new List<string>()));

            }
            return passed;
        }

        public bool CheckExistingEmail(string email)
        {
            if(email == null)
            {
                return false;
            }

            StudentDAO sd = StudentDAO.GetInstance();
            TutorDAO td = TutorDAO.GetInstance();
            DirectorDAO dd = DirectorDAO.GetInstance();

            if (sd.GetStudent(email) != null)  //or other searches
                return true;
            if (td.GetTutor(email) != null)  //or other searches
                return true;
            if (dd.GetDirector(email) != null)  //or other searches
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
