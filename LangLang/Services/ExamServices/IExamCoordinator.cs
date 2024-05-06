using System.Collections.Generic;
using LangLang.Model;

namespace LangLang.Services.ExamServices;

public interface IExamCoordinator
{
    public List<Exam> GetAvailableExams(Student student);
    public ExamApplication ApplyForExam(Student student, Exam exam);
    public List<Exam> GetAppliedExams(string studentId);
    public Exam? GetAttendingExam(string studentId);

    public void Accept(string studentId, string examId);
    public List<Exam> GetFinishedExams(string studentId);
    public List<Student> GetAppliedStudents(string examId);
    public List<Student> GetAttendanceStudents(string examId);
    public void SendNotification(string message, string receiverId);
    public void CancelApplication(string applicationId);
    public void CancelApplication(string studentId, string examId);
    public void RemoveAttendee(string studentId);
    public void FinishExam(Exam exam);
    public void ConfirmExam(Exam exam);
    public void GenerateAttendance(string examId);
}