using System;
using System.Collections.Generic;
using LangLang.Domain.Model;

namespace LangLang.Application.UseCases.Exam;

public interface IExamService
{
    public List<Domain.Model.Exam> GetAllExams();

    public Domain.Model.Exam? GetExamById(string id);

    public List<Domain.Model.Exam> GetExamsByTutor(string tutorId);

    public Domain.Model.Exam AddExam(Tutor tutor, Language? language, LanguageLevel? languageLvl, DateOnly? examDate,
        TimeOnly? examTime, int classroomNumber, int maxStudents);

    public Domain.Model.Exam UpdateExam(string id, Language? language, LanguageLevel? languageLvl, DateOnly? examDate,
        TimeOnly? examTime, int classroomNumber, int maxStudents);
    public void UpdateExam(Domain.Model.Exam exam);

    public void DeleteExam(string id);

    public List<Domain.Model.Exam> GetAvailableExamsForStudent(Student student);

    public List<Domain.Model.Exam> FilterExams(List<Domain.Model.Exam> exams, Language? language = null, LanguageLevel? languageLvl = null,
        DateOnly? date = null);
    public void FinishExam(Domain.Model.Exam exam);
    public void ConfirmExam(Domain.Model.Exam exam);
}