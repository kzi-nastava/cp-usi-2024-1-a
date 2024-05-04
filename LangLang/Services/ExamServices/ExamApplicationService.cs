using System;
using System.Collections.Generic;
using System.Linq;
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

    /// <summary>
    /// Filters out exams for which the student has <b>NOT</b> already applied for from the given exam list.
    /// </summary>
    /// <param name="student">Student whose applications should be considered.</param>
    /// <param name="exams">Exams to be filtered.</param>
    /// <returns>List of exams that is provided as an argument without exams that the student has <b>NOT</b> already applied for.</returns>
    public List<Exam> FilterNotAppliedExams(Student student, List<Exam> exams)
    {
        HashSet<string> appliedExamIds = new();
        foreach (var application in GetExamApplicationsForStudent(student.Id))
        {
            appliedExamIds.Add(application.ExamId);
        }

        return exams.Where(exam => !appliedExamIds.Contains(exam.Id)).ToList();
    }
    
    /// <summary>
    /// Filters out pending exams for which the student has already applied for from the given exam list.
    /// Only exams with applications that are in <b>PENDING</b> status are kept.
    /// </summary>
    /// <param name="student">Student whose applications should be considered.</param>
    /// <param name="exams">Exams to be filtered.</param>
    /// <returns>List of exams that is provided as an argument without exams that the student has already applied for.</returns>
    public List<Exam> FilterAppliedExams(Student student, List<Exam> exams)
    {
        var applicationsForExams = GetExamApplicationsForStudent(student.Id)
            .ToDictionary(application => application.ExamId);

        return exams.Where(exam =>
            applicationsForExams.ContainsKey(exam.Id) &&
            applicationsForExams[exam.Id].ExamApplicationState == ExamApplication.State.Pending
        ).ToList();
    }
}