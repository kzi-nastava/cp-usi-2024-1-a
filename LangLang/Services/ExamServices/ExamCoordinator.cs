using System;
using System.Collections.Generic;
using LangLang.DTO;
using LangLang.Model;
using LangLang.Services.AuthenticationServices;
using LangLang.Services.NotificationServices;
using LangLang.Services.UserServices;
using LangLang.Stores;

namespace LangLang.Services.ExamServices;

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

    public List<Exam> GetAvailableExams(Student student)
    {
        var examCandidates = _examService.GetAvailableExamsForStudent(student);
        return _examApplicationService.FilterNotAppliedExams(student, examCandidates);
    }
    public ExamApplication ApplyForExam(Student student, Exam exam)
    {
        var attendances = _examAttendanceService.GetAttendancesForStudent(student.Id);
        foreach (var attendace in attendances)
        {
            var foundExam = _examService.GetExamById(attendace.ExamId);
            if (foundExam != null && foundExam.ExamState != Exam.State.Reported)
                throw new Exception("Student cannot apply to multiple exams");
        }
        return _examApplicationService.ApplyForExam(student, exam);
    }

    public List<Exam> GetAppliedExams(Student student)
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
        //_examApplicationService.PauseStudentApplications(application);
        _examAttendanceService.AddAttendance(application.StudentId, application.ExamId);
        _examApplicationService.AcceptApplication(application);
        Exam? exam = _examService.GetExamById(application.ExamId);
        exam!.AddAttendance();
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

            Profile? receiver = _userProfileMapper.GetProfile(new UserDto(_studentService.GetStudentById(receiverId), UserType.Student));
            Profile? sender = _authenticationStore.CurrentUserProfile;

            if (receiver == null)
            {
                throw new ArgumentException("No receiver found");
            }
            _notificationService.AddNotification(message, receiver, sender);
        }

    }

    public void FinishExam(Exam exam)
    {
        //_examService.CalculateAverageScores
        _examService.FinishExam(exam);
        //student service add language skill
        //_examApplicationService.ActivateStudentApplications(studentId);
    }
    public void ConfirmExam(Exam exam)
        => _examService.ConfirmExam(exam);


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

    public Exam? GetAttendingExam(string studentId)
    {
        var examAttendance = _examAttendanceService.GetStudentAttendance(studentId)!;
        if (examAttendance == null) return null;
        return _examService.GetExamById(examAttendance.ExamId);
    }

    public List<Exam> GetFinishedExams(string studentId)
    {
        List<ExamAttendance> attendances = _examAttendanceService.GetFinishedExamsStudent(studentId);
        List<Exam> finishedExams = new();
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

    public void RemoveAttendee(string studentId)
    {
        var attendingExam = GetAttendingExam(studentId);
        if (attendingExam == null) return;
        _examAttendanceService.RemoveAttendee(studentId, attendingExam.Id);
    }

    public bool CanBeModified(string examId)
    {
        Exam? exam = _examService.GetExamById(examId);
        if (exam == null)
        {
            return false;
        }
        if (exam.ExamState != Exam.State.NotStarted)
        {
            return false;
        }
        return true;
    }
}