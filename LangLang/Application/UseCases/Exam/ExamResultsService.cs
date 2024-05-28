using LangLang.Application.DTO;
using LangLang.Application.UseCases.User;
using LangLang.Application.Utility;
using LangLang.Application.Utility.Email;
using LangLang.Domain.Model;
using LangLang.WPF.ViewModels.Tutor.Exam;

namespace LangLang.Application.UseCases.Exam;

public class ExamResultsService : IExamResultsService
{
    IExamAttendanceService _examAttendanceService;
    IStudentService _studentService;
    IAccountService _accountService;
    IEmailService _emailService;
    IExamService _examService;
    
    public ExamResultsService(IExamAttendanceService examAttendanceService, IStudentService studentService, IAccountService accountService, IEmailService emailService, IExamService examService) 
    {
        _examAttendanceService = examAttendanceService;
        _studentService = studentService;
        _accountService = accountService;
        _emailService = emailService;
        _examService = examService;
    }

    public EmailSendingResultDto SendExamResult(ExamViewModel examViewModel)
    {
        var attendences = _examAttendanceService.GetAttendancesForExam(examViewModel.Id);
        string examLanguage = $"{examViewModel.LanguageName} - {examViewModel.LanguageLevel}";
        string examDate = examViewModel.Date.ToString("dd/MM/yyyy");

        EmailSendingResultDto sendingResult = new();

        foreach (var attendence in attendences)
        {
            Student? student = _studentService.GetStudentById(attendence.StudentId);
            if (student == null)
                continue;
            string studentName = student.Name + " " + student.Surname;
            string email = _accountService.GetEmailByUserId(student.Id, UserType.Student);
            string emailSubject = string.Format(ExamEmailConstants.ResultEmailSubject, examLanguage);
            if (!attendence.IsGraded)
                continue;
            string emailBody;
            ExamGrade grade = attendence.Grade!;
            if (grade.IsPassing())
                emailBody = string.Format(ExamEmailConstants.ResultEmailBodyPass, studentName, examLanguage,
                    examDate, grade.ReadingScore, grade.WritingScore, grade.ListeningScore, grade.SpeakingScore);
            else
                emailBody = string.Format(ExamEmailConstants.ResultEmailBodyFail, studentName, examLanguage, examDate);
            try
            {
                _emailService.SendEmail(email, emailSubject, emailBody);
            }
            catch
            {
                sendingResult.FailedToSend.Add(student);
                continue;
            }
            sendingResult.SuccessfullySent.Add(student);
        }
        return sendingResult;
    }
}

