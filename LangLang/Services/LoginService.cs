using Consts;
using System;
using System.Data;
using System.Windows;
using System.Net.Mail;
using System.Linq;
using System.Windows.Controls;
using LangLang.Model;

public class LoginService
{
    //Singleton
    private static LoginService instance;
    public bool validUser = false;
    public bool validEmail = false;

    private LoginService()
    {
    }

    public static LoginService GetInstance()
    {
        if (instance == null)
        {
            instance = new LoginService();
        }
        return instance;
    }


    public void LogIn(string email, string password)
    {
        //in order to differentiate between non-existing email and just incorrect password
        validUser = false;
        validEmail = false;

        LogInStudent(email, password);
        //LogInTutor(email, password);
        //LogInDirector(email, password);
    }


    private void LogInStudent(string email, string password)
    {
        StudentDAO sd = StudentDAO.GetInstance();
        Student student = sd.GetStudent(email);

        if (student != null)
        {
            if(student.Password != password)
            {
                validEmail = true;  //email is good but password failed
            }
            else
            {
                validUser = true;

                StudentService ss = StudentService.GetInstance();
                ss.LoggedUser = student;
            }

        }
    }

    public void RegisterStudent(string email, string password, string name, string surname, DateTime birthDay, Gender gender, string phoneNumber, string qualification)
    {
        StudentDAO sd = StudentDAO.GetInstance();
        bool passed = CheckUserData(email, password, name, surname, phoneNumber);
        passed = !(CheckExistingEmail(email)); 

        if (passed)
        {
            sd.AddStudent(new Student(email, password, name, surname, birthDay, gender, phoneNumber, qualification, 0, "", null, null));
        }
    }

    public bool CheckExistingEmail(string email)
    {
        StudentDAO sd = StudentDAO.GetInstance();
        //other daos

        if (sd.GetStudent(email) != null)  //or other searches
        {
            return true;
        }
        return false;
    }

    public bool CheckUserData(string email, string password, string name, string surname, string phoneNumber)
    {
        bool passed = true;
        try
        {
            _ = new MailAddress(email);
        }
        catch
        {
            passed = false;
        }

        passed = !(int.TryParse(name, out _));      //checking if it's solely letters
        passed = !(int.TryParse(surname, out _));
        passed = int.TryParse(phoneNumber, out _);  //checking if it's solely numeric

        passed = password.Length > 8;               //password must include numbers, an upper character and should be longer than 8
        passed = !(password.Any(char.IsDigit));
        passed = !(password.Any(char.IsUpper));
        return passed;
    }

}

