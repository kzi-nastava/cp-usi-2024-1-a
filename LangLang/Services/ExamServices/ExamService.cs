using System;
using System.Collections.Generic;
using System.Linq;
using Consts;
using LangLang.DAO;
using LangLang.Model;

namespace LangLang.Services.ExamServices;

public class ExamService : IExamService
{
    private readonly IExamDAO _examDao;
    private readonly ITutorDAO _tutorDao;
    private readonly ILanguageDAO _languageDao;

    public ExamService(IExamDAO examDao, ITutorDAO tutorDao, ILanguageDAO languageDao)
    {
        _examDao = examDao;
        _tutorDao = tutorDao;
        _languageDao = languageDao;
    }

    public List<Exam> GetAllExams()
    {
        var exams = _examDao.GetAllExams().Values.ToList();
        return UpdateExamStates(exams);
    }

    public Exam? GetExamById(string id)
    {
        var exam = _examDao.GetExamById(id);
        if (exam != null)
        {
            exam = UpdateExamStateBasedOnDateTime(exam);
            _examDao.UpdateExam(exam.Id, exam);
        }
        return exam;
    }

    public List<Exam> GetExamsByTutor(string tutorId)
    {
        Tutor? tutor = _tutorDao.GetTutor(tutorId);
        if (tutor == null)
        {
            return new List<Exam>();
        }

        List<string> examIds = tutor.Exams;
        return UpdateExamStates(_examDao.GetExamsForIds(examIds));
    }

    private bool IsExamValid(Language? language, LanguageLvl? languageLvl, DateOnly? examDate, TimeOnly? examTime, int classroomNumber, int maxStudents)
    {
        if (language == null || languageLvl == null || examDate == null || examTime == null || maxStudents <= 0 ||
            classroomNumber <= 0 || classroomNumber > Constants.ClassroomsNumber)
        {
            return false;
        }
        if (_languageDao.GetLanguageById(language.Name) == null)
        {
            return false;
        }
        if (examDate.Value.ToDateTime(examTime.Value).Subtract(Constants.LockedExamTime) < DateTime.Now)
        {
            return false;
        }

        return true;
    }

    public Exam AddExam(Tutor tutor, Language? language, LanguageLvl? languageLvl, DateOnly? examDate, TimeOnly? examTime, int classroomNumber, int maxStudents)
    {
        if (!IsExamValid(language, languageLvl, examDate, examTime, classroomNumber, maxStudents))
        {
            throw new ArgumentException("Invalid exam data");
        }

        DateTime dateTime = new DateTime(examDate!.Value.Year, examDate.Value.Month, examDate.Value.Day, examTime!.Value.Hour, examTime.Value.Minute, examTime.Value.Second);
        Exam exam = new Exam(language!, languageLvl!.Value, dateTime, classroomNumber, Exam.State.NotStarted, maxStudents);
        exam = _examDao.AddExam(exam);

        tutor.Exams.Add(exam.Id);
        _tutorDao.UpdateTutor(tutor);
        return exam;
    }

    public Exam UpdateExam(string id, Language? language, LanguageLvl? languageLvl, DateOnly? examDate, TimeOnly? examTime, int classroomNumber, int maxStudents)
    {
        if (!IsExamValid(language, languageLvl, examDate, examTime, classroomNumber, maxStudents))
        {
            throw new ArgumentException("Invalid exam data");
        }

        Exam? oldExam = _examDao.GetExamById(id);
        if (oldExam == null)
        {
            throw new ArgumentException("Exam not found");
        }

        if (oldExam.ExamState != Exam.State.NotStarted)
        {
            throw new ArgumentException("Exam cannot be updated at this state");
        }

        DateTime dateTime = new DateTime(examDate!.Value.Year, examDate.Value.Month, examDate.Value.Day, examTime!.Value.Hour, examTime.Value.Minute, examTime.Value.Second);
        Exam.State examState = Exam.State.NotStarted;
        if (dateTime - Constants.LockedExamTime < DateTime.Now)
        {
            examState = Exam.State.Locked;
        }

        Exam? exam = new Exam(id, language!, languageLvl!.Value, dateTime, classroomNumber, examState, maxStudents);
        exam = UpdateExamStateBasedOnDateTime(exam);
        
        exam = _examDao.UpdateExam(id, exam);
        if (exam == null)
        {
            throw new ArgumentException("Exam not found");
        }
        return exam;
    }

    public void DeleteExam(string id)
    {
        if (_examDao.GetExamById(id) == null)
        {
            throw new ArgumentException("Exam not found");
        }
        _examDao.DeleteExam(id);
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

    private List<Exam> UpdateExamStates(IEnumerable<Exam> exams)
    {
        var updatedExams = exams.Select(UpdateExamStateBasedOnDateTime).ToList();
        updatedExams.ForEach(exam => _examDao.UpdateExam(exam.Id, exam));
        return updatedExams;
    }
    
    private Exam UpdateExamStateBasedOnDateTime(Exam exam)
    {
        if (exam.ExamState is Exam.State.Canceled or Exam.State.Graded or Exam.State.Reported)
            return exam;
        exam.ExamState = GetExamStateBasedOnDateTime(exam.Time, exam.Date, exam.TimeOfDay);
        return exam;
    }

    private static Exam.State GetExamStateBasedOnDateTime(DateTime dateTime, DateOnly examDate, TimeOnly examTime)
    {
        DateTime examDateTime = examDate.ToDateTime(examTime);
        if (dateTime < examDateTime.Subtract(Constants.LockedExamTime))
            return Exam.State.NotStarted;
        if (dateTime < examDateTime.Subtract(Constants.ConfirmedExamTime))
            return Exam.State.Locked;
        if (dateTime < examDateTime)
            return Exam.State.Confirmed;
        if (dateTime < examDateTime.Add(Constants.ExamDuration))
            return Exam.State.InProgress;
        return Exam.State.Finished;
    }
}