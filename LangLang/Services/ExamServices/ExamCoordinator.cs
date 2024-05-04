using System.Collections.Generic;
using LangLang.Model;

namespace LangLang.Services.ExamServices;

public class ExamCoordinator : IExamCoordinator
{
    private readonly IExamService _examService;
    private readonly IExamApplicationService _examApplicationService;

    public ExamCoordinator(IExamService examService, IExamApplicationService examApplicationService)
    {
        _examService = examService;
        _examApplicationService = examApplicationService;
    }

    public List<Exam> GetAvailableExams(Student student)
    {
        var examCandidates = _examService.GetAvailableExamsForStudent(student);
        return _examApplicationService.FilterNotAppliedExams(student, examCandidates);
    }

    public ExamApplication ApplyForExam(Student student, Exam exam)
    {
        // TODO: call _examAttendanceService to check for attendance status
        // Student cannot apply for exam if they have an attendance on exam that is not in Exam.State.Reported state.

        return _examApplicationService.ApplyForExam(student, exam);
    }

    public List<Exam> GetAppliedExams(Student student)
    {
        var examCandidates = _examService.GetAllExams();
        return _examApplicationService.FilterAppliedExams(student, examCandidates);
    }

    public Exam? GetAttendingExam(Student student)
    {
        // TODO: Implement
        return null;
    }
}