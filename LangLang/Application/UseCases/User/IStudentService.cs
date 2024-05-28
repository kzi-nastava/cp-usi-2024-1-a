using System;
using System.Collections.Generic;
using LangLang.Domain.Model;

namespace LangLang.Application.UseCases.User;

public interface IStudentService
{
    public void UpdateStudent(Student student, string name, string surname, DateTime birthDate, Gender gender,
        string phoneNumber);
    public void DeleteAccount(Student student);
    public Student? GetStudentById(string studentId);
    public Student AddStudent(Student student);
    uint AddPenaltyPoint(Student student);
    void RemovePenaltyPoint(Student student);
    List<Student> GetAllStudents();
    public void AddLanguageSkill(Student student, Language language, LanguageLevel languageLevel);
    public void AddPassedLanguage(Student student, Language language, LanguageLevel languageLevel);
}