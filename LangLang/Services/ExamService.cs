using System;
using System.Collections.Generic;
using System.Linq;
using Consts;
using LangLang.DAO;
using LangLang.Model;

namespace LangLang.Services;

public class ExamService
{
    private readonly ExamDAO examDao = ExamDAO.GetInstance();
    private readonly TutorDAO tutorDao = TutorDAO.GetInstance();
    private readonly LanguageDAO languageDao = LanguageDAO.GetInstance();

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

    private bool IsExamValid(Language? language, LanguageLvl? languageLvl, DateOnly? examDate, TimeOnly? examTime, int maxStudents)
    {
        if(language == null || languageLvl == null || examDate == null || examTime == null || maxStudents <= 0)
        {
            return false;
        }
        if(languageDao.GetLanguageById(language.Code) == null)
        {
            return false;
        }
        if(examDate < DateOnly.FromDateTime(DateTime.Now))
        {
            return false;
        }
        if(examDate == DateOnly.FromDateTime(DateTime.Now) && examTime < TimeOnly.FromDateTime(DateTime.Now))
        {
            return false;
        }

        return true;
    }
    
    public Exam AddExam(Language? language, LanguageLvl? languageLvl, DateOnly? examDate, TimeOnly? examTime, int maxStudents)
    {
        if(!IsExamValid(language, languageLvl, examDate, examTime, maxStudents))
        {
            throw new ArgumentException("Invalid exam data");
        }

        int classroomNumber = 1; // TODO: Update after TimetableService implementation
        
        DateTime dateTime = new DateTime(examDate!.Value.Year, examDate.Value.Month, examDate.Value.Day, examTime!.Value.Hour, examTime.Value.Minute, examTime.Value.Second);
        Exam exam = new Exam(language!, languageLvl!.Value, dateTime, classroomNumber, maxStudents);
        return examDao.AddExam(exam);
    }
}