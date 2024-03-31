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

    

}

