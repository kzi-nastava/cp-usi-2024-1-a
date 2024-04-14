using System;
using Consts;

namespace LangLang.Services.UserServices;

public interface IStudentService
{
    public bool UpdateStudent(string password, string name, string surname, DateTime birthDate, Gender gender,
        string phoneNumber);

    public void DeleteMyAccount();

    public void ApplyForCourse(string courseId);
}