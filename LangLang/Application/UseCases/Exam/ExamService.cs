﻿using System;
using System.Collections.Generic;
using System.Linq;
using LangLang.Application.DTO;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;

namespace LangLang.Application.UseCases.Exam;

public class ExamService : IExamService
{
    private readonly IExamRepository _examRepository;
    private readonly ITutorRepository _tutorRepository;
    private readonly ILanguageRepository _languageRepository;

    public ExamService(IExamRepository examRepository, ITutorRepository tutorRepository,
        ILanguageRepository languageRepository)
    {
        _examRepository = examRepository;
        _tutorRepository = tutorRepository;
        _languageRepository = languageRepository;
    }

    public List<Domain.Model.Exam> GetAllExams()
    {
        var exams = _examRepository.GetAll();
        return UpdateExamStates(exams);
    }

    public Domain.Model.Exam? GetExamById(string id)
    {
        var exam = _examRepository.Get(id);
        if (exam == null) return null;

        exam.UpdateExamStateBasedOnCurrentDateTime();
        _examRepository.Update(exam.Id, exam);
        return exam;
    }

    public List<Domain.Model.Exam> GetExamsByTutor(string tutorId)
    {
        Tutor? tutor = _tutorRepository.Get(tutorId);
        if (tutor == null)
        {
            return new List<Domain.Model.Exam>();
        }

        List<string> examIds = tutor.Exams;
        return UpdateExamStates(_examRepository.Get(examIds));
    }

    private bool IsExamValid(ExamDto examDto)
    {
        if (!examDto.IsValid())
            return false;

        if (_languageRepository.Get(examDto.Language!.Name) == null)
            return false;

        return true;
    }

    public Domain.Model.Exam AddExam(ExamDto examDto)
    {
        if (!IsExamValid(examDto))
        {
            throw new ArgumentException("Invalid exam data");
        }

        DateTime dateTime = new DateTime(examDto.Date!.Value.Year, examDto.Date.Value.Month, examDto.Date.Value.Day,
            examDto.Time!.Value.Hour, examDto.Time.Value.Minute, examDto.Time.Value.Second);
        Domain.Model.Exam exam = new Domain.Model.Exam(examDto.Language!, examDto.LanguageLevel!.Value, dateTime,
            examDto.ClassroomNumber,
            Domain.Model.Exam.State.NotStarted, examDto.MaxStudents);
        exam = _examRepository.Add(exam);

        if (examDto.Tutor == null)
        {
            throw new ArgumentException("No tutor provided");
        }

        examDto.Tutor.Exams.Add(exam.Id);
        _tutorRepository.Update(examDto.Tutor.Id, examDto.Tutor);
        return exam;
    }

    public Domain.Model.Exam UpdateExam(ExamDto examDto)
    {
        if (!IsExamValid(examDto))
        {
            throw new ArgumentException("Invalid exam data");
        }

        if (examDto.Id == null)
        {
            throw new ArgumentException("No exam id provided");
        }

        var exam = _examRepository.Get(examDto.Id);
        if (exam == null)
        {
            throw new ArgumentException("Exam not found");
        }

        DateTime dateTime = new DateTime(examDto.Date!.Value.Year, examDto.Date.Value.Month, examDto.Date.Value.Day,
            examDto.Time!.Value.Hour, examDto.Time.Value.Minute, examDto.Time.Value.Second);

        exam.Update(
            examDto.Language!,
            examDto.LanguageLevel!.Value,
            dateTime,
            examDto.ClassroomNumber,
            examDto.MaxStudents
        );

        exam = _examRepository.Update(exam.Id, exam);
        if (exam == null)
        {
            throw new ArgumentException("Exam not found");
        }

        return exam;
    }

    public void UpdateExam(Domain.Model.Exam exam)
    {
        _examRepository.Update(exam.Id, exam);
    }

    public void DeleteExam(string id)
    {
        if (_examRepository.Get(id) == null)
        {
            throw new ArgumentException("Exam not found");
        }

        _examRepository.Delete(id);
    }

    public List<Domain.Model.Exam> GetAvailableExamsForStudent(Student student) =>
        GetAllExams().Where(exam => exam.IsAvailable(student)).ToList();

    public List<Domain.Model.Exam> FilterExams(Language? language = null, LanguageLevel? languageLvl = null,
        DateOnly? date = null) =>
        GetAllExams().Where(exam => exam.MatchesFilter(language, languageLvl, date)).ToList();

    private List<Domain.Model.Exam> UpdateExamStates(List<Domain.Model.Exam> exams)
    {
        foreach (var exam in exams)
        {
            exam.UpdateExamStateBasedOnCurrentDateTime();
            _examRepository.Update(exam.Id, exam);
        }

        return exams;
    }

    public void FinishExam(Domain.Model.Exam exam)
    {
        exam.Finish();
        UpdateExam(exam);
    }

    public void ConfirmExam(Domain.Model.Exam exam)
    {
        exam.Confirm();
        UpdateExam(exam);
    }
}