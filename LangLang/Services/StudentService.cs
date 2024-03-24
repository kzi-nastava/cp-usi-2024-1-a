using Consts;
using System;
using System.Collections.Generic;

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

    public void UpdateStudentData(string password, string name, string surname, DateTime birthDate, Gender gender, string phoneNumber)
    {
        LoginService loginService = LoginService.GetInstance();
        bool checkData = loginService.checkUserData(LoggedUser.Email, password, name, surname, phoneNumber);

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

}
