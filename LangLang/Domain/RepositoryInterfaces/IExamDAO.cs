using System;
using System.Collections.Generic;
using LangLang.Domain.Model;

namespace LangLang.Domain.RepositoryInterfaces;

public interface IExamDAO
{
    public Dictionary<string, Exam> GetAllExams();
    public Exam? GetExamById(string id);
    public List<Exam> GetExamsForIds(List<string> ids);
    public List<Exam> GetExamsByDate(DateOnly date);
    public Exam AddExam(Exam exam);
    public Exam? UpdateExam(string id, Exam exam);    
    public void DeleteExam(string id);
}