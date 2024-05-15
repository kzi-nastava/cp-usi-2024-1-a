using System.Collections.Generic;
using System.Linq;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;

namespace LangLang.Repositories.Json;

public class ExamApplicationRepository : AutoIdRepository<ExamApplication>, IExamApplicationRepository
{
    public ExamApplicationRepository(string filepath, string lastIdFilePath) : base(filepath, lastIdFilePath)
    {
    }
    
    public List<ExamApplication> GetAllExamApplications() => GetAll().Values.ToList();

    public ExamApplication? Get(string studentId, string examId)
    {
        var applications = GetAllExamApplications().Where(application =>
            application.StudentId == studentId && application.ExamId == examId
        ).ToList();
        if (applications.Count <= 0)
            return null;
        return applications[0];
    }

    public List<ExamApplication> GetByStudent(string studentId)
        => GetAllExamApplications().Where(application => application.StudentId == studentId).ToList();

    public List<ExamApplication> GetByExam(string examId)
        => GetAllExamApplications().Where(application => application.ExamId == examId).ToList();

    public List<ExamApplication> GetPendingExamApplicationsByExam(string examId)
        => GetAllExamApplications().Where(application => 
            application.ExamId == examId && application.ExamApplicationState == ExamApplication.State.Pending
        ).ToList();

    public List<ExamApplication> GetByStudentAndExam(string studentId, string examId)
        => GetAllExamApplications().Where(application =>
            application.StudentId == studentId && application.ExamId == examId
        ).ToList();
}