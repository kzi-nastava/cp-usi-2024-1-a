using LangLang.Model;

namespace LangLang.Services.ExamServices;

public class ExamCoordinator : IExamCoordinator
{
    private readonly IExamApplicationService _examApplicationService;

    public ExamCoordinator(IExamApplicationService examApplicationService)
    {
        _examApplicationService = examApplicationService;
    }

    public ExamApplication? ApplyForExam(Student student, Exam exam)
    {
        // TODO: call _examAttendanceService to check for attendance status
        // Student cannot apply for exam if they have an attendance on exam that is not in Exam.State.Reported state.
        try
        {
            return _examApplicationService.ApplyForExam(student, exam);
        }
        catch
        {
            return null;
        }
    }
}