﻿using System;
using System.Collections.Generic;
using LangLang.Application.DTO;
using LangLang.Domain.Model;

namespace LangLang.Application.UseCases.Exam;

public interface IExamService
{
    public List<Domain.Model.Exam> GetAllExams();

    public Domain.Model.Exam? GetExamById(string id);

    public List<Domain.Model.Exam> GetExamsByTutor(string tutorId);

    public Domain.Model.Exam AddExam(ExamDto examDto);

    public Domain.Model.Exam UpdateExam(ExamDto examDto);

    public Domain.Model.Exam? SetTutor(Domain.Model.Exam exam, Tutor tutor);

    public void UpdateExam(Domain.Model.Exam exam);

    public void DeleteExam(string id);

    public List<Domain.Model.Exam> GetAvailableExamsForStudent(Student student);

    public List<Domain.Model.Exam> FilterExams(Language? language = null, LanguageLevel? languageLvl = null,
        DateOnly? date = null);

    public List<Domain.Model.Exam> FilterExamsForPage(int pageNumber, int examsPerPage, Language? language = null,
        LanguageLevel? languageLvl = null,
        DateOnly? date = null);
    
    public void FinishExam(Domain.Model.Exam exam);
    
    public void ConfirmExam(Domain.Model.Exam exam);
    public void GradedExam(Domain.Model.Exam exam);
    public void ReportedExam(Domain.Model.Exam exam);
    
    public List<Domain.Model.Exam> GetExamsForTimePeriod(DateTime from, DateTime to);

    public List<Domain.Model.Exam> GetAllExamsForPage(int pageNumber, int examsPerPage);
    
    public List<Domain.Model.Exam> GetExamsByTutorForPage(string tutorId, int pageNumber, int examsPerPage);
}