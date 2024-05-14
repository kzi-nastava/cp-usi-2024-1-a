using System;
using System.Collections.Generic;
using System.Linq;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;
using LangLang.Repositories;

namespace LangLang.Application.UseCases.Exam;

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

    public List<Domain.Model.Exam> GetAllExams()
    {
        var exams = _examDao.GetAllExams().Values.ToList();
        return UpdateExamStates(exams);
    }

    public Domain.Model.Exam? GetExamById(string id)
    {
        var exam = _examDao.GetExamById(id);
        if (exam != null)
        {
            exam = UpdateExamStateBasedOnDateTime(exam);
            _examDao.UpdateExam(exam.Id, exam);
        }
        return exam;
    }

    public List<Domain.Model.Exam> GetExamsByTutor(string tutorId)
    {
        Tutor? tutor = _tutorDao.GetTutor(tutorId);
        if (tutor == null)
        {
            return new List<Domain.Model.Exam>();
        }

        List<string> examIds = tutor.Exams;
        return UpdateExamStates(_examDao.GetExamsForIds(examIds));
    }

    private bool IsExamValid(Language? language, LanguageLevel? languageLvl, DateOnly? examDate, TimeOnly? examTime, int classroomNumber, int maxStudents)
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

    public Domain.Model.Exam AddExam(Tutor tutor, Language? language, LanguageLevel? languageLvl, DateOnly? examDate, TimeOnly? examTime, int classroomNumber, int maxStudents)
    {
        if (!IsExamValid(language, languageLvl, examDate, examTime, classroomNumber, maxStudents))
        {
            throw new ArgumentException("Invalid exam data");
        }

        DateTime dateTime = new DateTime(examDate!.Value.Year, examDate.Value.Month, examDate.Value.Day, examTime!.Value.Hour, examTime.Value.Minute, examTime.Value.Second);
        Domain.Model.Exam exam = new Domain.Model.Exam(language!, languageLvl!.Value, dateTime, classroomNumber, Domain.Model.Exam.State.NotStarted, maxStudents);
        exam = _examDao.AddExam(exam);

        tutor.Exams.Add(exam.Id);
        _tutorDao.UpdateTutor(tutor);
        return exam;
    }

    public Domain.Model.Exam UpdateExam(string id, Language? language, LanguageLevel? languageLvl, DateOnly? examDate, TimeOnly? examTime, int classroomNumber, int maxStudents)
    {
        if (!IsExamValid(language, languageLvl, examDate, examTime, classroomNumber, maxStudents))
        {
            throw new ArgumentException("Invalid exam data");
        }

        Domain.Model.Exam? oldExam = _examDao.GetExamById(id);
        if (oldExam == null)
        {
            throw new ArgumentException("Exam not found");
        }

        if (oldExam.ExamState != Domain.Model.Exam.State.NotStarted)
        {
            throw new ArgumentException("Exam cannot be updated at this state");
        }

        DateTime dateTime = new DateTime(examDate!.Value.Year, examDate.Value.Month, examDate.Value.Day, examTime!.Value.Hour, examTime.Value.Minute, examTime.Value.Second);
        Domain.Model.Exam.State examState = Domain.Model.Exam.State.NotStarted;
        if (dateTime - Constants.LockedExamTime < DateTime.Now)
        {
            examState = Domain.Model.Exam.State.Locked;
        }

        Domain.Model.Exam? exam = new Domain.Model.Exam(id, language!, languageLvl!.Value, dateTime, classroomNumber, examState, maxStudents);
        exam = UpdateExamStateBasedOnDateTime(exam);
        
        exam = _examDao.UpdateExam(id, exam);
        if (exam == null)
        {
            throw new ArgumentException("Exam not found");
        }
        return exam;
    }
    public void UpdateExam(Domain.Model.Exam exam)
    {
        _examDao.UpdateExam(exam.Id, exam);
    }

    public void DeleteExam(string id)
    {
        if (_examDao.GetExamById(id) == null)
        {
            throw new ArgumentException("Exam not found");
        }
        _examDao.DeleteExam(id);
    }

    public List<Domain.Model.Exam> GetAvailableExamsForStudent(Student student)
    {
        List<Domain.Model.Exam> exams = new();
        foreach (Domain.Model.Exam exam in GetAllExams())
        {
            if (exam.ExamState != Domain.Model.Exam.State.NotStarted)
            {
                continue;
            }
            if (exam.IsFull())
            {
                continue;
            }
            if (!student.CompletedCourseLanguages.ContainsKey(exam.Language.Name) ||
                student.CompletedCourseLanguages[exam.Language.Name] < exam.LanguageLevel)
            {
                continue;
            }
            if (student.PassedExamLanguages.ContainsKey(exam.Language.Name) &&
                student.PassedExamLanguages[exam.Language.Name] >= exam.LanguageLevel)
            {
                continue;
            }
            exams.Add(exam);
        }

        return exams;
    }

    public List<Domain.Model.Exam> FilterExams(List<Domain.Model.Exam> exams, Language? language = null, LanguageLevel? languageLvl = null, DateOnly? date = null)
    {
        List<Domain.Model.Exam> filteredExams = new();
        foreach (Domain.Model.Exam exam in exams)
        {
            if (language != null && !Equals(exam.Language, language))
            {
                continue;
            }
            if (languageLvl != null && exam.LanguageLevel != languageLvl)
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

    private List<Domain.Model.Exam> UpdateExamStates(IEnumerable<Domain.Model.Exam> exams)
    {
        var updatedExams = exams.Select(UpdateExamStateBasedOnDateTime).ToList();
        updatedExams.ForEach(exam => _examDao.UpdateExam(exam.Id, exam));
        return updatedExams;
    }
    
    private Domain.Model.Exam UpdateExamStateBasedOnDateTime(Domain.Model.Exam exam)
    {
        if (exam.ExamState is Domain.Model.Exam.State.Canceled or Domain.Model.Exam.State.Graded or Domain.Model.Exam.State.Reported)
            return exam;
        exam.ExamState = GetExamStateBasedOnDateTime(DateTime.Now, exam.Date, exam.TimeOfDay);
        return exam;
    }

    private static Domain.Model.Exam.State GetExamStateBasedOnDateTime(DateTime dateTime, DateOnly examDate, TimeOnly examTime)
    {
        DateTime examDateTime = examDate.ToDateTime(examTime);
        if (dateTime < examDateTime.Subtract(Constants.LockedExamTime))
            return Domain.Model.Exam.State.NotStarted;
        if (dateTime < examDateTime.Subtract(Constants.ConfirmedExamTime))
            return Domain.Model.Exam.State.Locked;
        if (dateTime < examDateTime)
            return Domain.Model.Exam.State.Confirmed;
        if (dateTime < examDateTime.Add(Constants.ExamDuration))
            return Domain.Model.Exam.State.InProgress;
        return Domain.Model.Exam.State.Finished;
    }
    public void FinishExam(Domain.Model.Exam exam)
    {
        exam.ExamState = Domain.Model.Exam.State.Finished;
        UpdateExam(exam);
    }
    public void ConfirmExam(Domain.Model.Exam exam)
    {
        exam.ExamState = Domain.Model.Exam.State.Confirmed;
        UpdateExam(exam);
    }
}