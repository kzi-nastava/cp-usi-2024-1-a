using LangLang.Application.UseCases.User;
using LangLang.Application.Utility.Email;
using LangLang.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LangLang.Application.UseCases.Course;

public class BestStudentsByCourseService: IBestStudentsByCourseService
{
    private readonly ICourseAttendanceService _courseAttendanceService;
    private readonly IEmailService _emailService;
    private readonly IAccountService _accountService;
    private readonly ICourseService _courseService;
    private readonly IStudentService _studentService;
    public BestStudentsByCourseService(ICourseAttendanceService courseAttendanceService, ICourseService courseService, IStudentService studentService, IEmailService emailService, IAccountService accountService) { 
        _courseAttendanceService = courseAttendanceService;
        _accountService = accountService;
        _courseService = courseService;
        _studentService = studentService;
        _emailService = emailService;   
    }

    public void SendEmailToBestStudents(string courseId, Utility.BestStudentsConstants.GradingPriority priority)
    {
        List<string> studentIds = GetBestStudents(courseId, priority);
        SendEmailToStudents(studentIds, courseId);
    }

    private List<string> GetBestStudents(string courseId, Utility.BestStudentsConstants.GradingPriority priority)
    {
        List<CourseAttendance> attendances = _courseAttendanceService.GetAttendancesForCourse(courseId);
        Dictionary<string, uint> studentRanks = new Dictionary<string, uint>();
        foreach (CourseAttendance attendance in attendances)
        {
            if(priority == Utility.BestStudentsConstants.GradingPriority.KnowledgeGrade)
            {
                studentRanks[attendance.StudentId] = (uint)attendance.ActivityGrade * Utility.BestStudentsConstants.ActivityGradeWeight;
                studentRanks[attendance.StudentId] = (uint)attendance.KnowledgeGrade * Utility.BestStudentsConstants.KnowledgeGradeWeight * Utility.BestStudentsConstants.PriorityFactor;
                studentRanks[attendance.StudentId] = (uint)(attendance.PenaltyPoints * Utility.BestStudentsConstants.PenaltyPointWeight);
            }else if(priority == Utility.BestStudentsConstants.GradingPriority.ActivityGrade)
            {
                studentRanks[attendance.StudentId] = (uint)attendance.ActivityGrade * Utility.BestStudentsConstants.ActivityGradeWeight * Utility.BestStudentsConstants.PriorityFactor;
                studentRanks[attendance.StudentId] = (uint)attendance.KnowledgeGrade * Utility.BestStudentsConstants.KnowledgeGradeWeight;
                studentRanks[attendance.StudentId] = (uint)(attendance.PenaltyPoints * Utility.BestStudentsConstants.PenaltyPointWeight);
            }
            else
            {
                studentRanks[attendance.StudentId] = (uint)attendance.ActivityGrade * Utility.BestStudentsConstants.ActivityGradeWeight;
                studentRanks[attendance.StudentId] = (uint)attendance.KnowledgeGrade * Utility.BestStudentsConstants.KnowledgeGradeWeight;
                studentRanks[attendance.StudentId] = (uint)(attendance.PenaltyPoints * Utility.BestStudentsConstants.PenaltyPointWeight);
            }
        }
        if (studentRanks.Count == 0)
        {
            throw new ArgumentException("No students attended the course.");
        }

        studentRanks = studentRanks.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        return studentRanks.Keys.Take((int)Utility.BestStudentsConstants.NumOfBestStudents).ToList();
    }   

    private void SendEmailToStudents(List<string> studentIds, string courseId)
    {
        string courseName = _courseService.GetCourseById(courseId)!.Name;

        foreach (string studentId in studentIds)
        {
            Student student = _studentService.GetStudentById(studentId)!;
            if (student != null)
            {
                string studentName = student.Name + " " + student.Surname;
                string email = _accountService.GetEmailByUserId(studentId);
                string emailSubject = "Congratulations on Your Outstanding Performance!";
                string emailBody = string.Format(Utility.BestStudentsConstants.bestStudentsAppreciationEmailBody, studentName, courseName);
                _emailService.SendEmail(email, emailSubject, emailBody);
            }


        }
    }

}
