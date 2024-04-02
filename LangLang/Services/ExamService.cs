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
        Exam.State examState = Exam.State.NotStarted;
        if(dateTime - Constants.ConfirmableExamTime < DateTime.Now)
        {
            examState = Exam.State.Confirmable;
        }
        Exam exam = new Exam(language!, languageLvl!.Value, dateTime, classroomNumber, examState, maxStudents);
        return examDao.AddExam(exam);
    }
    
    public Exam UpdateExam(string id, Language? language, LanguageLvl? languageLvl, DateOnly? examDate, TimeOnly? examTime, int maxStudents)
    {
        if(!IsExamValid(language, languageLvl, examDate, examTime, maxStudents))
        {
            throw new ArgumentException("Invalid exam data");
        }

        int classroomNumber = 1; // TODO: Update after TimetableService implementation

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
            if (exam.NumStudents >= exam.MaxStudents)
            {
                continue;
            }
            if (student.GetAppliedExams().Contains(exam.Id))
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