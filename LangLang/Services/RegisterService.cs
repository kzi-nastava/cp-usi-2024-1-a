using Consts;
using LangLang.DAO;
using LangLang.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LangLang.Services
{
    public class RegisterService
    {
        public static bool RegisterStudent(string email, string password, string name, string surname, DateTime birthDay, Gender gender, string phoneNumber, EducationLvl educationLvl)
        {
            StudentDAO sd = StudentDAO.GetInstance();

            bool passed = CheckUserData(email, password, name, surname, phoneNumber);
            passed &= !(CheckExistingEmail(email));

            if(birthDay == DateTime.MinValue)
            {
                return false;
            }

            if (passed)
            {
                sd.AddStudent(new Student(email, password, name, surname, birthDay, gender, phoneNumber, educationLvl, 0, "", "", null, null, null));
                return true;
            }
            return false;
        }



        public static bool CheckExistingEmail(string email)
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

        public static bool CheckUserData(string email, string password, string name, string surname, string phoneNumber)
        {
            if(email == null || password == null || name == null || surname == null || phoneNumber == null)
            {
                return false;
            }

            bool passed = true;
            try
            {
                _ = new MailAddress(email);
            }
            catch
            {
                passed = false;
            }

            passed &= int.TryParse(phoneNumber, out _);  //checking if it's solely numeric
            passed &= !name.Any(char.IsDigit);
            passed &= !surname.Any(char.IsDigit);
            passed &= password.Length >= 8;               //password must include numbers, an upper character and should be longer than 8
            passed &= password.Any(char.IsDigit);
            passed &= password.Any(char.IsUpper);
            passed &= phoneNumber.Length >= 6;
            return passed;
        }



    }
}
