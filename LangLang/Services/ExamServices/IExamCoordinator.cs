using LangLang.Model;

namespace LangLang.Services.ExamServices;

public interface IExamCoordinator
{
    public ExamApplication? ApplyForExam(Student student, Exam exam);
}