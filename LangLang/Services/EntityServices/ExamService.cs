using System;
using System.Collections.Generic;
using System.Linq;
using Consts;
using LangLang.DAO;
using LangLang.Model;

namespace LangLang.Services.EntityServices;

public class ExamService
{
    private readonly ExamDAO examDao = ExamDAO.GetInstance();
    private readonly TutorDAO tutorDao = TutorDAO.GetInstance();
    private readonly LanguageDAO languageDao = LanguageDAO.GetInstance();

    public List<Exam> GetAllExams()
    {
        return examDao.GetAllExams().Values.ToList();
    }

    public Exam? GetExamById(string id)
    {
        return examDao.GetExamById(id);
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

    private bool IsExamValid(Language? language, LanguageLvl? languageLvl, DateOnly? examDate, TimeOnly? examTime, int classroomNumber, int maxStudents)
    {
        if (language == null || languageLvl == null || examDate == null || examTime == null || maxStudents <= 0 ||
            classroomNumber <= 0 || classroomNumber > Constants.ClassroomsNumber)
        {
            return false;
        }
        if(languageDao.GetLanguageById(language.Name) == null)
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
    
    public Exam AddExam(Tutor tutor, Language? language, LanguageLvl? languageLvl, DateOnly? examDate, TimeOnly? examTime, int classroomNumber, int maxStudents)
    {
        if(!IsExamValid(language, languageLvl, examDate, examTime, classroomNumber, maxStudents))
        {
            throw new ArgumentException("Invalid exam data");
        }
        
        DateTime dateTime = new DateTime(examDate!.Value.Year, examDate.Value.Month, examDate.Value.Day, examTime!.Value.Hour, examTime.Value.Minute, examTime.Value.Second);
        Exam.State examState = Exam.State.NotStarted;
        if(dateTime - Constants.ConfirmableExamTime < DateTime.Now)
        {
            examState = Exam.State.Confirmable;
        }
        Exam exam = new Exam(language!, languageLvl!.Value, dateTime, classroomNumber, examState, maxStudents);
        exam = examDao.AddExam(exam);
        
        tutor.Exams.Add(exam.Id);
        tutorDao.UpdateTutor(tutor);
        return exam;
    }
    
    public Exam UpdateExam(string id, Language? language, LanguageLvl? languageLvl, DateOnly? examDate, TimeOnly? examTime, int classroomNumber, int maxStudents)
    {
        if(!IsExamValid(language, languageLvl, examDate, examTime, classroomNumber, maxStudents))
        {
            throw new ArgumentException("Invalid exam data");
        }

        Exam? oldExam = examDao.GetExamById(id);
        if (oldExam == null)
        {
            throw new ArgumentException("Exam not found");
        }
        
        if(oldExam.ExamState != Exam.State.NotStarted && oldExam.ExamState != Exam.State.Confirmable)
        {
            throw new ArgumentException("Exam cannot be updated at this state");
        }
        
        DateTime dateTime = new DateTime(examDate!.Value.Year, examDate.Value.Month, examDate.Value.Day, examTime!.Value.Hour, examTime.Value.Minute, examTime.Value.Second);
        Exam.State examState = Exam.State.NotStarted;
        if(dateTime - Constants.ConfirmableExamTime < DateTime.Now)
        {
            examState = Exam.State.Confirmable;
        }
        
        Exam? exam = new Exam(id, language!, languageLvl!.Value, dateTime, classroomNumber, examState, maxStudents);
        exam = examDao.UpdateExam(id, exam);
        if (exam == null)
        {
            throw new ArgumentException("Exam not found");
        }
        return exam;
    }
    
    public void DeleteExam(string id)
    {
        if (examDao.GetExamById(id) == null)
        {
            throw new ArgumentException("Exam not found");
        }
        examDao.DeleteExam(id);
    }

    public List<Exam> GetAvailableExamsForStudent(Student student)
    {
        List<Exam> exams = new();
        foreach (Exam exam in GetAllExams())
        {
            if (exam.ExamState != Exam.State.NotStarted)
            {
                continue;
            }
            if (exam.IsFull())
            {
                continue;
            }
            // TODO: Only allow exams for languages with finished courses
            exams.Add(exam);
        }

        return exams;
    }

    public List<Exam> FilterExams(List<Exam> exams, Language? language = null, LanguageLvl? languageLvl = null, DateOnly? date = null)
    {
        List<Exam> filteredExams = new();
        foreach (Exam exam in exams)
        {
            if (language != null && !Equals(exam.Language, language))
            {
                continue;
            }
            if (languageLvl != null && exam.LanguageLvl != languageLvl)
            {
                continue;
            }
            if (date != null && exam.Date != date)
            {
                continue;
            }
            filteredExams.Add(exam);
        }

        return filteredExams;
    }
}