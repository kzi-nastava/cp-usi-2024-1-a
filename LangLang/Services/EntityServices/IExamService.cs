using System;
using System.Collections.Generic;
using Consts;
using LangLang.Model;

namespace LangLang.Services.EntityServices;

public interface IExamService
{
    public List<Exam> GetAllExams();
    
    public Exam? GetExamById(string id);
    
    public List<Exam> GetExamsByTutor(string tutorId);
    
    public Exam AddExam(Tutor tutor, Language? language, LanguageLvl? languageLvl, DateOnly? examDate,
        TimeOnly? examTime, int classroomNumber, int maxStudents);

    public Exam UpdateExam(string id, Language? language, LanguageLvl? languageLvl, DateOnly? examDate,
        TimeOnly? examTime, int classroomNumber, int maxStudents);

    public void DeleteExam(string id);

    public List<Exam> GetAvailableExamsForStudent(Student student);

    public List<Exam> FilterExams(List<Exam> exams, Language? language = null, LanguageLvl? languageLvl = null,
        DateOnly? date = null);
}