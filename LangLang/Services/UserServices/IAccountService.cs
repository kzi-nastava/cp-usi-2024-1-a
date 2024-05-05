using Consts;
using System;
using LangLang.DTO;
using LangLang.Model;
using System.Collections.Generic;

namespace LangLang.Services.UserServices;
public interface IAccountService
{
    public void UpdateStudent(string studentId, string password, string name, string surname, DateTime birthDate, Gender gender, string phoneNumber);
    public void DeleteStudent(Student student);
    public void DeactivateStudentAccount(Student student);
    void RegisterStudent(RegisterStudentDto registerDto);
    Tutor RegisterTutor(RegisterTutorDto registerDto);
    public Tutor UpdateTutor(string tutorId, string password, string name, string surname, DateTime birthDate, Gender gender, string phoneNumber, List<Tuple<Language, LanguageLvl>> knownLanguages, DateTime dateAdded);
    public void DeleteTutor(Tutor tutor);
}

