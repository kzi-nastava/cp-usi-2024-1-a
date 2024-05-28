using System.Collections.Generic;
using LangLang.Domain.Model;

namespace LangLang.Application.UseCases.Exam;

public interface IExamApplicationService
{
    public ExamApplication? GetExamApplication(string id);
    public ExamApplication? GetExamApplication(string studentId, string examId);
    public List<ExamApplication> GetExamApplicationsForStudent(string studentId);
    public List<ExamApplication> GetExamApplications(string examId);
    public List<ExamApplication> GetPendingExamApplications(string examId);
    public ExamApplication ApplyForExam(Student student, Domain.Model.Exam exam);
    public ExamApplication AcceptApplication(ExamApplication application);
    public ExamApplication RejectApplication(ExamApplication application);
    public void CancelApplication(ExamApplication application);
    public List<Domain.Model.Exam> FilterAppliedExams(Student student, List<Domain.Model.Exam> exams);
    public List<Domain.Model.Exam> FilterNotAppliedExams(Student student, List<Domain.Model.Exam> exams);
    public void DeleteApplications(string studentId);
    public void DeleteApplicationsForExam(string examId);
}