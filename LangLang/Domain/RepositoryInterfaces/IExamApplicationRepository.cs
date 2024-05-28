using System.Collections.Generic;
using LangLang.Domain.Model;

namespace LangLang.Domain.RepositoryInterfaces;

public interface IExamApplicationRepository : IRepository<ExamApplication>
{
    public ExamApplication? Get(string studentId, string examId);
    public List<ExamApplication> GetByStudent(string studentId);
    public List<ExamApplication> GetByExam(string examId);
    public List<ExamApplication> GetPendingExamApplicationsByExam(string examId);
    public List<ExamApplication> GetByStudentAndExam(string studentId, string examId);
}