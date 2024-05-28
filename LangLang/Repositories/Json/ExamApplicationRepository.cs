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

    public ExamApplication? Get(string studentId, string examId)
    {
        var applications = GetAll().Where(application =>
            application.StudentId == studentId && application.ExamId == examId
        ).ToList();
        if (applications.Count <= 0)
            return null;
        return applications[0];
    }

    public List<ExamApplication> GetByStudent(string studentId)
        => GetAll().Where(application => application.StudentId == studentId).ToList();

    public List<ExamApplication> GetByExam(string examId)
        => GetAll().Where(application => application.ExamId == examId).ToList();

    public List<ExamApplication> GetPendingExamApplicationsByExam(string examId)
        => GetAll().Where(application => 
            application.ExamId == examId && application.ExamApplicationState == ExamApplication.State.Pending
        ).ToList();

    public List<ExamApplication> GetByStudentAndExam(string studentId, string examId)
        => GetAll().Where(application =>
            application.StudentId == studentId && application.ExamId == examId
        ).ToList();
}