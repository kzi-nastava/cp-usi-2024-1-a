using System;
using System.Collections.Generic;
using LangLang.Application.DTO;
using LangLang.Domain.Model;

namespace LangLang.Application.UseCases.User;
public interface IAccountService
{
    public void UpdateStudent(string studentId, string password, string name, string surname, DateTime birthDate, Gender gender, string phoneNumber);
    public void DeleteStudent(Student student);
    public void DeactivateStudentAccount(Student student);
    void RegisterStudent(RegisterStudentDto registerDto);
    Tutor RegisterTutor(RegisterTutorDto registerDto);
    public Tutor UpdateTutor(string tutorId, string password, string name, string surname, DateTime birthDate, Gender gender, string phoneNumber, List<Tuple<Language, LanguageLevel>> knownLanguages, DateTime dateAdded);
    public void DeleteTutor(Tutor tutor);
    public string GetEmailByUserId(string userId, UserType userType);
}

