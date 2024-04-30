using Consts;
using LangLang.Services.CourseServices;
using System;


namespace LangLang.Services.UserServices
{
    public class AccountService : IAccountService
    {
        private readonly IStudentService _studentService;
        private readonly IStudentCourseCoordinator _studentCourseCoordinator;

        public AccountService(IStudentService studentService, IStudentCourseCoordinator studentCourseCoordinator) 
        {
            _studentService = studentService;
            _studentCourseCoordinator = studentCourseCoordinator;
        }

        public void UpdateStudent(string studentId, string password, string name, string surname, DateTime birthDate, Gender gender, string phoneNumber)
        {
            if (_studentCourseCoordinator.GetStudentAttendingCourse(studentId) != null)
            {
                throw new ArgumentException("Student applied for courses, editing profile not allowed");
            }
            _studentService.UpdateStudent(_studentService.GetStudentById(studentId)!, password, name, surname, birthDate, gender, phoneNumber);
        }

        public void DeleteStudent(string studentId)
        {
            _studentCourseCoordinator.RemoveAttendee(studentId);
            //exam coordinator
            _studentService.DeleteAccount(_studentService.GetStudentById(studentId)!);
        }
    }
}
