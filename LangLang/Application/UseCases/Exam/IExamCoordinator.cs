using System.Collections.Generic;
using LangLang.Domain.Model;

namespace LangLang.Application.UseCases.Exam;

public interface IExamCoordinator
{
    public List<Domain.Model.Exam> GetAvailableExams(Student student);
    public ExamApplication ApplyForExam(Student student, Domain.Model.Exam exam);
    public List<Domain.Model.Exam> GetAppliedExams(Student student);
    public Domain.Model.Exam? GetAttendingExam(string studentId);

    public void Accept(string studentId, string examId);
    public List<Domain.Model.Exam> GetFinishedExams(string studentId);
    public List<Student> GetAppliedStudents(string examId);
    public List<Student> GetAttendanceStudents(string examId);
    public void SendNotification(string? message, string receiverId);
    public void CancelApplication(string applicationId);
    public void CancelApplication(string studentId, string examId);
    public void RemoveAttendee(string studentId);
    public void FinishExam(Domain.Model.Exam exam);
    public void ConfirmExam(Domain.Model.Exam exam);
    public void GradedExam(Domain.Model.Exam exam);
    public void ReportedExam(Domain.Model.Exam exam, List<Student> passed);
    public void GenerateAttendance(string examId);
    List<ExamAttendance> GetGradedAttendancesForLastYear();
    public void DeleteExamsByTutor(Tutor tutor);
    public List<Domain.Model.Exam> GetGradedExams();
}