using System.Collections.Generic;
using System.Linq;
using LangLang.DAO;
using LangLang.Model;

namespace LangLang.Services;

public class ExamService
{
    private readonly ExamDAO examDao = ExamDAO.GetInstance();
    private readonly TutorDAO tutorDao = TutorDAO.GetInstance();

    public List<Exam> GetAllExams()
    {
        return examDao.GetAllExams().Values.ToList();
    }

    public List<Exam> GetExamsByTutor(string tutorId)
    {
        Tutor? tutor = tutorDao.GetTutor(tutorId);
        if (tutor == null)
        {
            return new List<Exam>();
        }

        List<string> examIds = tutor.Exams;

        return examDao.GetExamsForIds(examIds);
    }

    public void AddExam(Exam exam)
    {
        // TODO: validate
        examDao.AddExam(exam);
    }
}