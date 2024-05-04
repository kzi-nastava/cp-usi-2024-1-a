using System.Collections.Generic;
using LangLang.Model;

namespace LangLang.Services.ExamServices;

public interface IExamCoordinator
{
    public List<Exam> GetAvailableExams(Student student);
    public ExamApplication ApplyForExam(Student student, Exam exam);
    public List<Exam> GetAppliedExams(Student student);
    public Exam? GetAttendingExam(Student student);
}