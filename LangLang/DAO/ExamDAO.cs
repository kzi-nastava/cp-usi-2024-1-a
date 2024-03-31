using System.Collections.Generic;
using System.IO;
using Consts;
using LangLang.Model;
using LangLang.Util;

namespace LangLang.DAO;

public class ExamDAO
{
    private static ExamDAO? instance;
    private Dictionary<string, Exam>? exams;
    private readonly LastIdDAO lastIdDao = LastIdDAO.GetInstance();

    private Dictionary<string, Exam> Exams
    {
        get
        {
            if (exams == null)
            {
                Load();
            }

            return exams!;
        }
        set => exams = value;
    }

    private ExamDAO()
    {
    }

    public static ExamDAO GetInstance()
    {
        return instance ??= new ExamDAO();
    }

    public Dictionary<string, Exam> GetAllExams()
    {
        return Exams;
    }

    public Exam? GetExamById(string id)
    {
        return Exams.GetValueOrDefault(id);
    }

    public void AddExam(Exam exam)
    {
        lastIdDao.IncrementExamId();
        exam.Id = lastIdDao.GetExamId();
        Exams.Add(exam.Id, exam);
        Save();
    }

    public void UpdateExam(string id, Exam exam)
    {
        if (Exams.ContainsKey(id))
        {
            Exams[id] = exam;
        }
        Save();
    }

    public void DeleteExam(string id)
    {
        Exams.Remove(id);
        Save();
    }

    private void Load()
    {
        try
        {
            exams = JsonUtil.ReadFromFile<Exam>(Constants.ExamFilePath);
        }
        catch (DirectoryNotFoundException)
        {
            Exams = new Dictionary<string, Exam>();
            Save();
        }
        catch (FileNotFoundException)
        {
            Exams = new Dictionary<string, Exam>();
            Save();
        }
    }

    private void Save()
    {
        JsonUtil.WriteToFile(Exams, Constants.ExamFilePath);
    }
}