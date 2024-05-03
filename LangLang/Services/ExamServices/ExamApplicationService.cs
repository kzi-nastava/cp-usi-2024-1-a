using System;
using System.Collections.Generic;
using LangLang.DAO;
using LangLang.Model;

namespace LangLang.Services.ExamServices;

public class ExamApplicationService : IExamApplicationService
{
    private readonly IExamApplicationDAO _examApplicationDao;

    public ExamApplicationService(IExamApplicationDAO examApplicationDao)
    {
        _examApplicationDao = examApplicationDao;
    }

    public ExamApplication? GetExamApplication(string id) => _examApplicationDao.GetExamApplication(id);

    public List<ExamApplication> GetExamApplicationsForStudent(string studentId)
        => _examApplicationDao.GetExamApplicationsByStudent(studentId);

    public List<ExamApplication> GetExamApplications(string examId)
        => _examApplicationDao.GetExamApplicationsByExam(examId);

    public List<ExamApplication> GetPendingExamApplications(string examId)
        => _examApplicationDao.GetPendingExamApplicationsByExam(examId);

    public ExamApplication ApplyForExam(Student student, Exam exam)
    {
        if (exam.IsFull())
            throw new ArgumentException("No place available at exam.");
        return _examApplicationDao.AddExamApplication(new ExamApplication(student.Id, exam.Id));
    }

    public ExamApplication AcceptApplication(ExamApplication application)
    {
        if (application.ExamApplicationState != ExamApplication.State.Pending)
            throw new ArgumentException("Cannot accept application that is not pending.");
        application.ExamApplicationState = ExamApplication.State.Accepted;
        return _examApplicationDao.UpdateExamApplication(application.Id, application) ??
               throw new ArgumentException("No application with the given id.");
    }

    public ExamApplication RejectApplication(ExamApplication application)
    {
        if (application.ExamApplicationState != ExamApplication.State.Pending)
            throw new ArgumentException("Cannot reject application that is not pending.");
        application.ExamApplicationState = ExamApplication.State.Rejected;
        return _examApplicationDao.UpdateExamApplication(application.Id, application) ??
               throw new ArgumentException("No application with the given id.");
    }

    public void CancelApplication(ExamApplication application)
    {
        if (application.ExamApplicationState != ExamApplication.State.Pending)
            throw new ArgumentException($"Cannot cancel application with state {application.ExamApplicationState}.");
        _examApplicationDao.DeleteExamApplication(application.Id);
    }
}