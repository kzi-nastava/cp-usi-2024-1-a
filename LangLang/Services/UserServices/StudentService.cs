using System;
using Consts;
using LangLang.DAO;
using LangLang.Model;
using LangLang.Services.EntityServices;

namespace LangLang.Services.UserServices
{
    public class StudentService : IStudentService
    {
        private readonly IStudentDAO _studentDao;
        private readonly IExamService _examService;
        private readonly ICourseService _courseService;
        
        public StudentService(IStudentDAO studentDao, IExamService examService, ICourseService courseService)
        {
            _studentDao = studentDao;
            _examService = examService;
            _courseService = courseService;
        }

        //Return if the updating is successful
        public bool UpdateStudent(Student student, string password, string name, string surname, DateTime birthDate, Gender gender, string phoneNumber)
        {
            if (AttendingCourse(student))
            {
                return false;
            }
            student.Name = name;
            student.Surname = surname;
            student.Password = password;
            student.Gender = gender;
            student.BirthDate = birthDate;
            student.Gender = gender;
            student.PhoneNumber = phoneNumber;

            _studentDao.AddStudent(student);
            return true;
        }

        private static bool AttendingCourse(Student student)
        {
            return student.AttendingCourse != "" || student.AttendingExam != "" || student.GetAppliedCourses().Count != 0 || student.GetAppliedExams().Count != 0;
        }

    
        public void DeleteAccount(Student student)
        {
            CancelCourses(student);
            CancelExams(student);
            _studentDao.DeleteStudent(student.Email);
        }
    

        public void ApplyForCourse(Student student, string courseId)
        {
            student.AddCourse(courseId);
            Course? course = _courseService.GetCourseById(courseId);
            course!.AddAttendance();
            _courseService.UpdateCourse(course);
        }

        private void CancelCourses(Student student)
        {
            foreach (string courseID in student.GetAppliedCourses())
            {
                Course? course = _courseService.GetCourseById(courseID);
                course!.CancelAttendance();
                _courseService.UpdateCourse(course);
            }
        }

        private void CancelExams(Student student)
        {
            foreach (string examID in student.GetAppliedExams())
            {
                Exam? exam = _examService.GetExamById(examID);
                exam?.CancelAttendance();
                //examService.UpdateExam(exam!);
                //TODO handle exam updating
            }
        }


    }

}
