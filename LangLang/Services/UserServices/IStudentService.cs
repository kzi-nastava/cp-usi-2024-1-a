using System;
using System.Collections.Generic;
using Consts;
using LangLang.Model;

namespace LangLang.Services.UserServices;

public interface IStudentService
{
    public bool UpdateStudent(Student student, string password, string name, string surname, DateTime birthDate, Gender gender,
        string phoneNumber);
    public List<Course> GetFinishedCourses(Student student);
    public bool AppliedForCourse(Student student, string courseId);
    public bool AppliedForExam(Student student, string examId);
    public void DeleteAccount(Student student);
    public void ApplyForCourse(Student student, string courseId);
    public void CancelCourse(Student student);
    public void CancelCourseApplication(Student student, string courseID);
    
}