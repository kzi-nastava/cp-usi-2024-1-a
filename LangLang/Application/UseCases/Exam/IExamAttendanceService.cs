using System.Collections.Generic;
using LangLang.Application.DTO;
using LangLang.Domain.Model;

namespace LangLang.Application.UseCases.Exam;
public interface IExamAttendanceService
{
    public List<ExamAttendance> GetAttendancesForStudent(string studentId);
    public List<ExamAttendance> GetAttendancesForExam(string examId);
    public ExamAttendance? GetStudentAttendance(string studentId);
    public List<ExamAttendance> GetFinishedExamsStudent(string studentId);
    public ExamAttendance AddAttendance(string studentId, string examId);
    public void RemoveAttendee(string studentId, string examId);
    public void GradeStudent(string studentId, string examId, ExamGradeDto examGradeDto);
    public void RateTutor(ExamAttendance attendance, int rating);
    public List<Student> GetStudents(string examId);
    public ExamAttendance? GetAttendance(string studentId, string examId);
    public void AddPassedLanguagesToStudents(Domain.Model.Exam exam);
    public bool AvailableForApplication(Domain.Model.Exam exam, Student student);
}

