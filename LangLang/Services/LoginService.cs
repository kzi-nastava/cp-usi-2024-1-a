using Consts;
using System;
using System.Data;
using System.Windows;
using System.Net.Mail;
using System.Linq;
using System.Windows.Controls;
using LangLang.Model;
using LangLang.DAO;
using LangLang.Services;

public class LoginService
{
    //Singleton
    private static LoginService? instance;
    public bool validUser = false;
    public bool validEmail = false;
    public Type userType;

    private LoginService()
    {
    }

    public static LoginService GetInstance()
    {
        return instance ??= new LoginService();
    }


    public void LogIn(string email, string password)
    {
        //in order to differentiate between non-existing email and just incorrect password
        validUser = false;
        validEmail = false;

        if (email == null || email == "" || password == null || password == "")
            return;

        LogInDirector(email, password);
        if (validUser) return;
        LogInTutor(email, password);
        if (validUser) return;
        LogInStudent(email, password);
    }


    private void LogInStudent(string email, string password)
    {
        Student? student = StudentDAO.GetInstance().GetStudent(email);

        if (student != null)
        {
            if(student.Password != password)
                validEmail = true;  //email is good but password failed
            else
            {
                validUser = true;

                StudentService.GetInstance().LoggedUser = student;
                userType = typeof(Student);
            }
        }
    }
    private void LogInTutor(string email, string password)
    {
        Tutor? tutor = TutorDAO.GetInstance().GetTutor(email);

        if (tutor != null)
        {
            if (tutor.Password != password)
                validEmail = true;  //email is good but password failed
            else
            {
                validUser = true;

                TutorService.GetInstance().LoggedUser = tutor;
                userType = typeof(Tutor);
            }
        }
    }
    private void LogInDirector(string email, string password)
    {
        Director? director = DirectorDAO.GetInstance().GetDirector(email);

        if (director != null)
        {
            if (director.Password != password)
                validEmail = true;  //email is good but password failed
            else
            {
                validUser = true;

                DirectorService.GetInstance().LoggedUser = director;
                userType = typeof(Director);
            }
        }
    }


}

