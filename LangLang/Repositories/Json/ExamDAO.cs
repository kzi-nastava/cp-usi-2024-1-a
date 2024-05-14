using System;
using System.Collections.Generic;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;
using LangLang.Repositories.Json.Util;

namespace LangLang.Repositories.Json;

public class ExamDAO : IExamDAO
{
    private Dictionary<string, Exam>? _exams;
    private readonly ILastIdDAO _lastIdDao;

    private Dictionary<string, Exam> Exams
    {
        get
        {
            _exams ??= JsonUtil.ReadFromFile<Exam>(Constants.ExamFilePath);
            return _exams!;
        }
        set => _exams = value;
    }

    public ExamDAO(ILastIdDAO lastIdDao)
    {
        _lastIdDao = lastIdDao;
    }

    public Dictionary<string, Exam> GetAllExams()
    {
        return Exams;
    }

    public Exam? GetExamById(string id)
    {
        return Exams.GetValueOrDefault(id);
    }
    
    public List<Exam> GetExamsForIds(List<string> ids)
    {
        List<Exam> exams = new();
        foreach (string id in ids)
        {
            if (Exams.ContainsKey(id))
            {
                exams.Add(Exams[id]);
            }
        }
        return exams;
    }
    
    public List<Exam> GetExamsByDate(DateOnly date)
    {
        List<Exam> exams = new();
        foreach (Exam exam in GetAllExams().Values)
        {
            if (exam.Date == date)
            {
                exams.Add(exam);
            }
        }

        return exams;
    }

    public Exam AddExam(Exam exam)
    {
        _lastIdDao.IncrementExamId();
        exam.Id = _lastIdDao.GetExamId();
        Exams.Add(exam.Id, exam);
        SaveExams();
        return exam;
    }

    public Exam? UpdateExam(string id, Exam exam)
    {
        if (!Exams.ContainsKey(id)) return null;
        Exams[id] = exam;
        SaveExams();
        return exam;
    }

    public void DeleteExam(string id)
    {
        Exams.Remove(id);
        SaveExams();
    }

    private void SaveExams()
    {
        JsonUtil.WriteToFile(Exams, Constants.ExamFilePath);
    }
}