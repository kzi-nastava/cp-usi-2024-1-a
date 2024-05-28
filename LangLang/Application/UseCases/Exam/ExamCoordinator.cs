using System;
using System.Collections.Generic;
using System.Linq;
using LangLang.Application.DTO;
using LangLang.Application.Stores;
using LangLang.Application.UseCases.User;
using LangLang.Application.Utility.Authentication;
using LangLang.Application.Utility.Notification;
using LangLang.Domain.Model;

namespace LangLang.Application.UseCases.Exam;

public class ExamCoordinator : IExamCoordinator
{
    private readonly IExamService _examService;
    private readonly IExamApplicationService _examApplicationService;
    private readonly IStudentService _studentService;
    private readonly IExamAttendanceService _examAttendanceService;
    private readonly IUserProfileMapper _userProfileMapper;
    private readonly INotificationService _notificationService;
    private readonly IAuthenticationStore _authenticationStore;

    public ExamCoordinator(IExamService examService, IExamApplicationService examApplicationService,
            IStudentService studentService, IExamAttendanceService examAttendanceService, IUserProfileMapper userProfileMapper,
            INotificationService notificationService, IAuthenticationStore authenticationStore)
    {
        _examService = examService;
        _examApplicationService = examApplicationService;
        _authenticationStore = authenticationStore;
        _userProfileMapper = userProfileMapper;
        _examAttendanceService = examAttendanceService;
        _studentService = studentService;
        _notificationService = notificationService;
    }

    public List<Domain.Model.Exam> GetAvailableExams(Student student)
    {
        var examCandidates = _examService.GetAvailableExamsForStudent(student);
        return _examApplicationService.FilterNotAppliedExams(student, examCandidates);
    }
    public ExamApplication ApplyForExam(Student student, Domain.Model.Exam exam)
    {
        if(_examAttendanceService.AvailableForApplication(exam, student))
            return _examApplicationService.ApplyForExam(student, exam);
        else
            throw new Exception("Student cannot apply to multiple exams");
    }

    public List<Domain.Model.Exam> GetAppliedExams(Student student)
    {
        var examCandidates = _examService.GetAllExams();
        return _examApplicationService.FilterAppliedExams(student, examCandidates);
    }

    public void Accept(string studentId, string examId)
    {
        ExamApplication? application = _examApplicationService.GetExamApplication(studentId, examId);
        if (application == null)
        {
            throw new ArgumentException("No application found");
        }
        if (!CanBeModified(application.ExamId))
        {
            throw new ArgumentException("Cannot accept student at this state");
        }
        _examAttendanceService.AddAttendance(application.StudentId, application.ExamId);
        _examApplicationService.AcceptApplication(application);
        Domain.Model.Exam? exam = _examService.GetExamById(application.ExamId);
        exam!.AddAttendance();
        _examService.UpdateExam(exam);
    }

    public void CancelApplication(string applicationId)
    {
        ExamApplication? application = _examApplicationService.GetExamApplication(applicationId);
        if (application == null)
        {
            throw new ArgumentException("No application found");
        }
        if (!CanBeModified(application.ExamId))
        {
            throw new ArgumentException("Cannot accept student at this state");
        }
        _examApplicationService.CancelApplication(application);
    }

    public void CancelApplication(string studentId, string examId)
    {
        ExamApplication? application = _examApplicationService.GetExamApplication(studentId, examId);
        if (application == null)
        {
            throw new ArgumentException("No application found");
        }
        if (!CanBeModified(application.ExamId))
        {
            throw new ArgumentException("Cannot accept student at this state");
        }
        _examApplicationService.CancelApplication(application);
    }
    
    public void SendNotification(string? message, string receiverId)
    {
        if (message != null)
        {
            var receiver = _userProfileMapper.GetProfile(new UserDto(_studentService.GetStudentById(receiverId), UserType.Student));
            var sender = _authenticationStore.CurrentUserProfile;

            if (receiver == null)
            {
                throw new ArgumentException("No receiver found");
            }
            _notificationService.AddNotification(message, receiver, sender);
        }
    }

    public void FinishExam(Domain.Model.Exam exam)
    {
        //_examService.CalculateAverageScores
        _examService.FinishExam(exam);
        //_examAttendanceService.AddPassedLanguagesToStudents(exam);
    }
    public void ConfirmExam(Domain.Model.Exam exam)
        => _examService.ConfirmExam(exam);
    public void GradedExam(Domain.Model.Exam exam)
        => _examService.GradedExam(exam);
    public void ReportedExam(Domain.Model.Exam exam)
    {
        _examService.ReportedExam(exam);
        _examAttendanceService.AddPassedLanguagesToStudents(exam);
    }

    public List<Student> GetAppliedStudents(string examId)
    {
        List<ExamApplication> applications = _examApplicationService.GetPendingExamApplications(examId);
        List<Student> appliedStudents = new();
        foreach (ExamApplication application in applications)
        {
            appliedStudents.Add(_studentService.GetStudentById(application.StudentId)!);
        }
        return appliedStudents;
    }

    public List<Student> GetAttendanceStudents(string examId)
    {
        List<ExamAttendance> attendances = _examAttendanceService.GetAttendancesForExam(examId);
        List<Student> attendanceStudents = new();
        foreach (ExamAttendance attendance in attendances)
        {
            attendanceStudents.Add(_studentService.GetStudentById(attendance.StudentId)!);
        }
        return attendanceStudents;
    }

    public Domain.Model.Exam? GetAttendingExam(string studentId)
    {
        var examAttendance = _examAttendanceService.GetStudentAttendance(studentId);
        if (examAttendance == null) return null;
        return _examService.GetExamById(examAttendance.ExamId);
    }

    public List<Domain.Model.Exam> GetFinishedExams(string studentId)
    {
        List<ExamAttendance> attendances = _examAttendanceService.GetFinishedExamsStudent(studentId);
        List<Domain.Model.Exam> finishedExams = new();
        foreach (ExamAttendance attendance in attendances)
        {
            finishedExams.Add(_examService.GetExamById(attendance.ExamId)!);
        }
        return finishedExams;
    }

    public void GenerateAttendance(string examId)
    {
        List<ExamApplication> applications = _examApplicationService.GetExamApplications(examId);
        foreach (ExamApplication application in applications)
        {
            if (application.ExamApplicationState == ExamApplication.State.Accepted)
            {
                bool studentAttendingAnotherExam = (_examAttendanceService.GetAttendancesForStudent(application.StudentId)).Count != 0;
                if (!studentAttendingAnotherExam)
                {
                    _examAttendanceService.AddAttendance(application.StudentId, application.ExamId);
                    //_examApplicationService.CancelApplication(application);
                }
            }
        }
    }

    public List<ExamAttendance> GetGradedAttendancesForLastYear()
    {
        List<ExamAttendance> attendances = new();
        var exams = _examService.GetExamsForTimePeriod(DateTime.Now.AddYears(-1), DateTime.Now);
        foreach (var exam in exams)
        {
            attendances.AddRange(_examAttendanceService.GetAttendancesForExam(exam.Id)
                .Where(attendance => attendance.IsGraded));
        }
        return attendances;
    }

    public void DeleteExamsByTutor(Tutor tutor)
    {
        var exams = _examService.GetExamsByTutor(tutor.Id);
        foreach (var exam in exams)
        {
            _examApplicationService.DeleteApplicationsForExam(exam.Id);
            _examAttendanceService.DeleteAttendancesForExam(exam.Id);
        }
    }

    public void RemoveAttendee(string studentId)
    {
        var attendingExam = GetAttendingExam(studentId);
        if (attendingExam != null)
            _examAttendanceService.RemoveAttendee(studentId, attendingExam.Id);
        _examApplicationService.DeleteApplications(studentId);
    }

    private bool CanBeModified(string examId)
    {
        var exam = _examService.GetExamById(examId);
        if (exam == null)
        {
            return false;
        }
        return exam.CanBeUpdated();
    }

    public List<Domain.Model.Exam> GetGradedExams()
        => _examService.GetAllExams().Where(exam => exam.IsGraded()).ToList();
}