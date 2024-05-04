using System.Collections.Generic;
using System.Linq;
using Consts;
using LangLang.Model;
using LangLang.Util;

namespace LangLang.DAO.JsonDao;

public class ExamApplicationDAO : IExamApplicationDAO
{
    private readonly ILastIdDAO _lastIdDao;
    
    public ExamApplicationDAO(ILastIdDAO lastIdDao)
    {
        _lastIdDao = lastIdDao;
    }

    private Dictionary<string, ExamApplication>? _examApplications;

    private Dictionary<string, ExamApplication> ExamApplications
    {
        get
        {
            _examApplications ??= JsonUtil.ReadFromFile<ExamApplication>(Constants.ExamApplicationFilePath);
            return _examApplications!;
        }
        set => _examApplications = value;
    }

    public List<ExamApplication> GetAllExamApplications() => ExamApplications.Values.ToList();

    public ExamApplication? GetExamApplication(string id) => ExamApplications.GetValueOrDefault(id);

    public ExamApplication? GetExamApplication(string studentId, string examId)
    {
        var applications = GetAllExamApplications().Where(application =>
            application.StudentId == studentId && application.ExamId == examId
        ).ToList();
        if (applications.Count <= 0)
            return null;
        return applications[0];
    }

    public List<ExamApplication> GetExamApplicationsByStudent(string studentId)
        => GetAllExamApplications().Where(application => application.StudentId == studentId).ToList();

    public List<ExamApplication> GetExamApplicationsByExam(string examId)
        => GetAllExamApplications().Where(application => application.ExamId == examId).ToList();

    public List<ExamApplication> GetPendingExamApplicationsByExam(string examId)
        => GetAllExamApplications().Where(application => 
            application.ExamId == examId && application.ExamApplicationState == ExamApplication.State.Pending
        ).ToList();

    public List<ExamApplication> GetExamApplicationsByStudentAndExam(string studentId, string examId)
        => GetAllExamApplications().Where(application =>
            application.StudentId == studentId && application.ExamId == examId
        ).ToList();

    public ExamApplication AddExamApplication(ExamApplication examApplication)
    {
        _lastIdDao.IncrementExamApplicationId();
        examApplication.Id = _lastIdDao.GetExamApplicationId();
        ExamApplications.Add(examApplication.Id, examApplication);
        SaveExamApplications();
        return examApplication;
    }

    public ExamApplication? UpdateExamApplication(string id, ExamApplication examApplication)
    {
        if (!ExamApplications.ContainsKey(id)) return null;
        ExamApplications[id] = examApplication;
        SaveExamApplications();
        return examApplication;
    }

    public void DeleteExamApplication(string id)
    {
        ExamApplications.Remove(id);
        SaveExamApplications();
    }
    
    private void SaveExamApplications()
    {
        JsonUtil.WriteToFile(ExamApplications, Constants.ExamApplicationFilePath);
    }
}