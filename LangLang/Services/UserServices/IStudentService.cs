using System;
using System.Collections;
using System.Collections.Generic;
using Consts;
using LangLang.Model;

namespace LangLang.Services.UserServices;

public interface IStudentService
{
    public bool UpdateStudent(Student student, string name, string surname, DateTime birthDate, Gender gender,
        string phoneNumber);
    public void DeleteAccount(Student student);
    public Student? GetStudentById(string studentId);
    public Student AddStudent(Student student);
    uint AddPenaltyPoint(Student student);
    void RemovePenaltyPoint(Student student);
    List<Student> GetAllStudents();
    public void AddLanguageSkill(Student student, Language language, LanguageLvl languageLvl);
}