using System;
using System.Collections.Generic;
using System.Linq;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;

namespace LangLang.Application.UseCases.Exam;

public class ExamApplicationService : IExamApplicationService
{
    private readonly IExamApplicationRepository _examApplicationRepository;

    public ExamApplicationService(IExamApplicationRepository examApplicationRepository)
    {
        _examApplicationRepository = examApplicationRepository;
    }

    public ExamApplication? GetExamApplication(string id) => _examApplicationRepository.Get(id);

    public ExamApplication? GetExamApplication(string studentId, string examId)
        => _examApplicationRepository.Get(studentId, examId);

    public List<ExamApplication> GetExamApplicationsForStudent(string studentId)
        => _examApplicationRepository.GetByStudent(studentId);

    public List<ExamApplication> GetExamApplications(string examId)
        => _examApplicationRepository.GetByExam(examId);

    public List<ExamApplication> GetPendingExamApplications(string examId)
        => _examApplicationRepository.GetPendingExamApplicationsByExam(examId);

    public ExamApplication ApplyForExam(Student student, Domain.Model.Exam exam)
    {
        if (exam.IsFull())
            throw new ArgumentException("No place available at exam.");
        return _examApplicationRepository.Add(new ExamApplication(exam.Id, student.Id));
    }

    public ExamApplication AcceptApplication(ExamApplication application)
    {
        if (!application.Accept())
            throw new ArgumentException("Cannot accept application that is not pending.");
        return _examApplicationRepository.Update(application.Id, application) ??
               throw new ArgumentException("No application with the given id.");
    }

    public ExamApplication RejectApplication(ExamApplication application)
    {
        if (!application.Reject())
            throw new ArgumentException("Cannot reject application that is not pending.");
        return _examApplicationRepository.Update(application.Id, application) ??
               throw new ArgumentException("No application with the given id.");
    }

    public void CancelApplication(ExamApplication application)
    {
        if (application.ExamApplicationState != ExamApplication.State.Pending)
            throw new ArgumentException($"Cannot cancel application with state {application.ExamApplicationState}.");
        _examApplicationRepository.Delete(application.Id);
    }
    
    public void DeleteApplications(string studentId)
    {
        foreach (var application in GetExamApplicationsForStudent(studentId))
        {
            _examApplicationRepository.Delete(application.Id);
        }
    }

    public void DeleteApplicationsForExam(string examId)
    {
        foreach (var application in GetExamApplications(examId))
        {
            _examApplicationRepository.Delete(application.Id);
        }
    }

    /// <summary>
    /// Filters out exams for which the student has <b>NOT</b> already applied for from the given exam list.
    /// </summary>
    /// <param name="student">Student whose applications should be considered.</param>
    /// <param name="exams">Exams to be filtered.</param>
    /// <returns>List of exams that is provided as an argument without exams that the student has <b>NOT</b> already applied for.</returns>
    public List<Domain.Model.Exam> FilterNotAppliedExams(Student student, List<Domain.Model.Exam> exams)
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
    public List<Domain.Model.Exam> FilterAppliedExams(Student student, List<Domain.Model.Exam> exams)
    {
        var applicationsForExams = GetExamApplicationsForStudent(student.Id)
            .ToDictionary(application => application.ExamId);

        return exams.Where(exam =>
            applicationsForExams.ContainsKey(exam.Id) &&
            applicationsForExams[exam.Id].ExamApplicationState == ExamApplication.State.Pending
        ).ToList();
    }
}