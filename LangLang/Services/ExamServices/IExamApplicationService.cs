using System.Collections.Generic;
using LangLang.Model;

namespace LangLang.Services.ExamServices;

public interface IExamApplicationService
{
    public ExamApplication? GetExamApplication(string id);
    public ExamApplication? GetExamApplication(string studentId, string examId);
    public List<ExamApplication> GetExamApplicationsForStudent(string studentId);
    public List<ExamApplication> GetExamApplications(string examId);
    public List<ExamApplication> GetPendingExamApplications(string examId);
    public ExamApplication ApplyForExam(Student student, Exam exam);
    public ExamApplication AcceptApplication(ExamApplication application);
    public ExamApplication RejectApplication(ExamApplication application);
    public void CancelApplication(ExamApplication application);
    List<Exam> FilterAppliedExams(Student student, List<Exam> exams);
    List<Exam> FilterNotAppliedExams(Student student, List<Exam> exams);
}