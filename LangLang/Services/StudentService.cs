using Consts;
using System;
using System.Collections.Generic;
using System.Windows;
using LangLang.Model;

public class StudentService
{
	StudentDAO studentDAO = StudentDAO.GetInstance();
    public Student LoggedUser { get; set; }



    //Singleton
    private static StudentService instance;
    private StudentService()
    {
    }

    public static StudentService GetInstance()
    {
        if (instance == null)
        {
            instance = new StudentService();
        }
        return instance;
    }

    public void UpdateStudent(string password, string name, string surname, DateTime birthDate, Gender gender, string phoneNumber)
    {
        if (LoggedUser.AttendingCourse != "")
        {
            MessageBox.Show($"User is attending a course!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        LoginService loginService = LoginService.GetInstance();
        bool checkData = loginService.CheckUserData(LoggedUser.Email, password, name, surname, phoneNumber);
        if (checkData)
        {
            LoggedUser.Name = name;
            LoggedUser.Surname = surname;
            LoggedUser.BirthDate = birthDate;
            LoggedUser.Gender = gender;
            LoggedUser.PhoneNumber = phoneNumber;

            studentDAO.AddStudent(LoggedUser);  //since its a hashmap it will replace it
        }
       
    }

    /*
    public void DeleteMyAccount()
    {
        CourseService cs = CourseService.getInstance();
        for (string courseID: LoggedUser.GetAppliedCourses)
        {
            cs.removeStudent(LoggedUser.Email, courseID);
        }

        cs.removeApplied(LoggedUser.Email, LoggedUser.AttendingCourse);


        studentDAO.DeleteStudent(LoggedUser.Email);
    }
    */

    public void ApplyForCourse(string courseId)
    {
        LoggedUser.AddCourse(courseId);
        //Alert courses to add me
        //courseService.addStudent(LoggedUser.Email)
    }

}
