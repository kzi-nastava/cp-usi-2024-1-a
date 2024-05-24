using LangLang.Application.UseCases.User;
using LangLang.Application.Utility.Email;
using System.Collections.Generic;


namespace LangLang.Application.UseCases.Course;

public class BestStudentsByCourseService: IBestStudentsByCourseService
{
    private readonly ICourseAttendanceService _courseAttendanceService;
    private readonly IEmailService _emailService;
    private readonly IAccountService _accountService;
    public BestStudentsByCourseService(ICourseAttendanceService courseAttendanceService, IEmailService emailService, IAccountService accountService) { 
        _courseAttendanceService = courseAttendanceService;
        _accountService = accountService;
        _emailService = emailService;   
    }

    public void SendEmailToBestStudents(string courseId,Domain.Model.GradingPriority.Priority priority)
    {

    }

    private void SendEmailToStudents(List<Domain.Model.Student> students)
    {
        foreach(Domain.Model.Student student in students)
        {
            string email = _accountService.GetEmailByUserId(student.Id);
            string emailSubject = "Congrats";
            string emailBody = "Wow you did that";
            _emailService.SendEmail(email, emailSubject, emailBody);
        }
    }





}
