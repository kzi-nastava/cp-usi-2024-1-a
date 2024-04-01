using Consts;
using System;
using System.Collections.Generic;
using System.Windows;
using LangLang.Model;
using LangLang.Services;

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

    public bool UpdateStudent(string password, string name, string surname, DateTime birthDate, Gender gender, string phoneNumber)
    {
        if (LoggedUser.AttendingCourse != "" || LoggedUser.AttendingExam != "" || LoggedUser.GetAppliedCourses().Count != 0 || LoggedUser.GetAppliedExams().Count != 0)
        {
            //MessageBox.Show($"User is attending a course!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        LoggedUser.Name = name;
        LoggedUser.Surname = surname;
        LoggedUser.Password = password;
        LoggedUser.Gender = gender;
        LoggedUser.BirthDate = birthDate;
        LoggedUser.Gender = gender;
        LoggedUser.PhoneNumber = phoneNumber;

        studentDAO.AddStudent(LoggedUser);  //since its a hashmap it will replace it
        return true;
    }

    
    public void DeleteMyAccount()
    {
        CourseService cs = new();
        foreach (string courseID in LoggedUser.GetAppliedCourses())
        {
            Course course = cs.GetCourseById(courseID);
            course.CancelAttendance();
            cs.UpdateCourse(course);
        }

        foreach(string examID in LoggedUser.GetAppliedExams())
        {
            /**/
        }


        studentDAO.DeleteStudent(LoggedUser.Email);
    }
    

    public void ApplyForCourse(string courseId)
    {
        LoggedUser.AddCourse(courseId);
        CourseService cs = new();
        Course course = cs.GetCourseById(courseId);
        course.AddAttendance();
        cs.UpdateCourse(course);
    }

}
