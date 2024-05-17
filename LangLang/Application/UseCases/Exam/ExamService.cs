using System;
using System.Collections.Generic;
using System.Linq;
using LangLang.Application.DTO;
using LangLang.Domain;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;

namespace LangLang.Application.UseCases.Exam;

public class ExamService : IExamService
{
    private readonly IExamRepository _examRepository;
    private readonly ITutorRepository _tutorRepository;
    private readonly ILanguageRepository _languageRepository;

    public ExamService(IExamRepository examRepository, ITutorRepository tutorRepository,
        ILanguageRepository languageRepository)
    {
        _examRepository = examRepository;
        _tutorRepository = tutorRepository;
        _languageRepository = languageRepository;
    }

    public List<Domain.Model.Exam> GetAllExams()
    {
        var exams = _examRepository.GetAll();
        return UpdateExamStates(exams);
    }

    public Domain.Model.Exam? GetExamById(string id)
    {
        var exam = _examRepository.Get(id);
        if (exam != null)
        {
            exam = UpdateExamStateBasedOnDateTime(exam);
            _examRepository.Update(exam.Id, exam);
        }

        return exam;
    }

    public List<Domain.Model.Exam> GetExamsByTutor(string tutorId)
    {
        Tutor? tutor = _tutorRepository.Get(tutorId);
        if (tutor == null)
        {
            return new List<Domain.Model.Exam>();
        }

        List<string> examIds = tutor.Exams;
        return UpdateExamStates(_examRepository.Get(examIds));
    }

    private bool IsExamValid(ExamDto examDto)
    {
        if (examDto.Language == null || examDto.LanguageLevel == null || examDto.Date == null || examDto.Time == null ||
            examDto.MaxStudents <= 0 ||
            examDto.ClassroomNumber <= 0 || examDto.ClassroomNumber > Constants.ClassroomsNumber)
        {
            return false;
        }

        if (_languageRepository.Get(examDto.Language.Name) == null)
        {
            return false;
        }

        if (examDto.Date.Value.ToDateTime(examDto.Time.Value).Subtract(Constants.LockedExamTime) < DateTime.Now)
        {
            return false;
        }

        return true;
    }

    public Domain.Model.Exam AddExam(ExamDto examDto)
    {
        if (!IsExamValid(examDto))
        {
            throw new ArgumentException("Invalid exam data");
        }

        DateTime dateTime = new DateTime(examDto.Date!.Value.Year, examDto.Date.Value.Month, examDto.Date.Value.Day,
            examDto.Time!.Value.Hour, examDto.Time.Value.Minute, examDto.Time.Value.Second);
        Domain.Model.Exam exam = new Domain.Model.Exam(examDto.Language!, examDto.LanguageLevel!.Value, dateTime,
            examDto.ClassroomNumber,
            Domain.Model.Exam.State.NotStarted, examDto.MaxStudents);
        exam = _examRepository.Add(exam);

        if (examDto.Tutor == null)
        {
            throw new ArgumentException("No tutor provided");
        }

        examDto.Tutor.Exams.Add(exam.Id);
        _tutorRepository.Update(examDto.Tutor.Id, examDto.Tutor);
        return exam;
    }

    public Domain.Model.Exam UpdateExam(ExamDto examDto)
    {
        if (!IsExamValid(examDto))
        {
            throw new ArgumentException("Invalid exam data");
        }

        if (examDto.Id == null)
        {
            throw new ArgumentException("No exam id provided");
        }

        var oldExam = _examRepository.Get(examDto.Id);
        if (oldExam == null)
        {
            throw new ArgumentException("Exam not found");
        }

        if (oldExam.ExamState != Domain.Model.Exam.State.NotStarted)
        {
            throw new ArgumentException("Exam cannot be updated at this state");
        }

        DateTime dateTime = new DateTime(examDto.Date!.Value.Year, examDto.Date.Value.Month, examDto.Date.Value.Day,
            examDto.Time!.Value.Hour, examDto.Time.Value.Minute, examDto.Time.Value.Second);
        var examState = Domain.Model.Exam.State.NotStarted;
        if (dateTime - Constants.LockedExamTime < DateTime.Now)
        {
            examState = Domain.Model.Exam.State.Locked;
        }

        var exam = new Domain.Model.Exam(
            examDto.Id!,
            examDto.Language!,
            examDto.LanguageLevel!.Value,
            dateTime,
            examDto.ClassroomNumber,
            examState,
            examDto.MaxStudents
        );
        exam = UpdateExamStateBasedOnDateTime(exam);

        exam = _examRepository.Update(exam.Id, exam);
        if (exam == null)
        {
            throw new ArgumentException("Exam not found");
        }

        return exam;
    }

    public void UpdateExam(Domain.Model.Exam exam)
    {
        _examRepository.Update(exam.Id, exam);
    }

    public void DeleteExam(string id)
    {
        if (_examRepository.Get(id) == null)
        {
            throw new ArgumentException("Exam not found");
        }

        _examRepository.Delete(id);
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

    public List<Domain.Model.Exam> FilterExams(Language? language = null, LanguageLevel? languageLvl = null,
        DateOnly? date = null) =>
        GetAllExams().Where(exam => exam.MatchesFilter(language, languageLvl, date)).ToList();

    private List<Domain.Model.Exam> UpdateExamStates(IEnumerable<Domain.Model.Exam> exams)
    {
        var updatedExams = exams.Select(UpdateExamStateBasedOnDateTime).ToList();
        updatedExams.ForEach(exam => _examRepository.Update(exam.Id, exam));
        return updatedExams;
    }

    private Domain.Model.Exam UpdateExamStateBasedOnDateTime(Domain.Model.Exam exam)
    {
        if (exam.ExamState is Domain.Model.Exam.State.Canceled or Domain.Model.Exam.State.Graded
            or Domain.Model.Exam.State.Reported)
            return exam;
        exam.ExamState = GetExamStateBasedOnDateTime(DateTime.Now, exam.Date, exam.TimeOfDay);
        return exam;
    }

    private static Domain.Model.Exam.State GetExamStateBasedOnDateTime(DateTime dateTime, DateOnly examDate,
        TimeOnly examTime)
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