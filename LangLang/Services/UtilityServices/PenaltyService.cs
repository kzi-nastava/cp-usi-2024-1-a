using System;
using LangLang.Model;
using LangLang.Services.UserServices;
using Consts;
using LangLang.DTO;
using LangLang.Services.AuthenticationServices;

namespace LangLang.Services.UtilityServices;

public class PenaltyService : IPenaltyService
{
    private readonly IStudentService _studentService;
    private readonly IAccountService _accountService;
    private readonly IUserProfileMapper _userProfileMapper;

    public PenaltyService(IStudentService studentService, IAccountService accountService, IUserProfileMapper userProfileMapper)
    {
        _studentService = studentService;
        _accountService = accountService;
        _userProfileMapper = userProfileMapper;
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
        
        // TODO: _notificationService.AddNotification(message, studentProfile, senderProfile);
    }
}