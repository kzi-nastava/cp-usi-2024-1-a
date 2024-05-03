using System.Collections.Generic;
using LangLang.Model;

namespace LangLang.DAO;

public interface IExamApplicationDAO
{
    public List<ExamApplication> GetAllExamApplications();
    public ExamApplication? GetExamApplication(string id);
    public List<ExamApplication> GetExamApplicationsByStudent(string studentId);
    public List<ExamApplication> GetExamApplicationsByExam(string examId);
    public List<ExamApplication> GetPendingExamApplicationsByExam(string examId);
    public List<ExamApplication> GetExamApplicationsByStudentAndExam(string studentId, string examId);
    public ExamApplication AddExamApplication(ExamApplication examApplication);
    public ExamApplication? UpdateExamApplication(string id, ExamApplication examApplication);
    public void DeleteExamApplication(string id);
}