using LangLang.Application.DTO;
using LangLang.Application.UseCases.Course;
using LangLang.Application.UseCases.User;
using LangLang.Application.Utility.Authentication;
using LangLang.Application.Utility.Notification;
using LangLang.Domain;
using LangLang.Domain.Model;

namespace LangLang.Application.UseCases.Common;

public class PenaltyService : IPenaltyService
{
    private readonly IStudentService _studentService;
    private readonly IAccountService _accountService;
    private readonly IUserProfileMapper _userProfileMapper;
    private readonly INotificationService _notificationService;
    private readonly ICourseAttendanceService _courseAttendanceService;

    public PenaltyService(IStudentService studentService, IAccountService accountService, IUserProfileMapper userProfileMapper, INotificationService notificationService, ICourseAttendanceService courseAttendanceService)
    {
        _studentService = studentService;
        _accountService = accountService;
        _userProfileMapper = userProfileMapper;
        _notificationService = notificationService;
        _courseAttendanceService = courseAttendanceService;
    }

    public void AddPenaltyPoint(Student student, Person? sender = null)
    {
        var numPoints = _studentService.AddPenaltyPoint(student);
        
        SendNotification(student, sender, numPoints);
        
        if (numPoints >= Constants.PenaltyPointLimit)
        {
            _accountService.DeactivateStudentAccount(student);  
        }
    }

    public void AddPenaltyPoint(Student student, Domain.Model.Course course, Person? sender = null)
    {
        _courseAttendanceService.AddPenaltyPoint(student.Id, course.Id);
        AddPenaltyPoint(student, sender);
    }

    public void RemovePenaltyPoints()
    {
        foreach (var student in _studentService.GetAllStudents())
        {
            _studentService.RemovePenaltyPoint(student);
        }
    }
    
    private void SendNotification(Student student, Person? sender, uint numPoints)
    {
        var studentProfile = _userProfileMapper.GetProfile(new UserDto(student, UserType.Student));
        if (studentProfile == null) return;
        
        var senderProfile = _userProfileMapper.GetProfile(new UserDto(sender, UserType.Tutor));
        
        var message = $"You have been given one penalty point and now have a total of {numPoints} penalty points.";
        
        _notificationService.AddNotification(message, studentProfile, senderProfile);
    }
}