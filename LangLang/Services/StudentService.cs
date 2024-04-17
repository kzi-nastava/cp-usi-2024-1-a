using Consts;
using System;
using LangLang.Model;
using System.Collections.Generic;

namespace LangLang.Services
{
    public class StudentService
    {
        readonly StudentDAO studentDAO = StudentDAO.GetInstance();
        private readonly ExamService examService = new();
        private readonly CourseService courseService = new();
        public Student? LoggedUser { get; set; }

        //Singleton
        private static StudentService? instance;
        private StudentService() 
        {
        }

        public static StudentService GetInstance()
        {
            return instance ??= new StudentService();
        }

        //Return if the updating is successfull 
        public bool UpdateStudent(string password, string name, string surname, DateTime birthDate, Gender gender, string phoneNumber)
        {
            if (AttendingCourse(LoggedUser!))
            {
                return false;
            }
            LoggedUser!.Name = name;
            LoggedUser.Surname = surname;
            LoggedUser.Password = password;
            LoggedUser.Gender = gender;
            LoggedUser.BirthDate = birthDate;
            LoggedUser.Gender = gender;
            LoggedUser.PhoneNumber = phoneNumber;

            studentDAO.AddStudent(LoggedUser);
            return true;
        }

        private static bool AttendingCourse(Student student)
        {
            return student.AttendingCourse != "" || student.AttendingExam != "" || student.GetAppliedCourses().Count != 0 || student.GetAppliedExams().Count != 0;
        }

        public List<Course> GetFinishedCourses(Student student)
        {
            List<Course> courseList = new List<Course>();
            foreach(string courseId in student.GetFinishedCourses()){
                courseList.Add(courseService.GetCourseById(courseId)!);
            }
            return courseList;
        }

        public bool AppliedForCourse(Student student, string courseId)
        {
            List<string> appliedCourses = student.GetAppliedCourses();
            return appliedCourses.Contains(courseId);
        }

        public bool AppliedForExam(Student student, string examId)
        {
            List<string> appliedExams = student.GetAppliedExams();
            return appliedExams.Contains(examId);
        }

    
        public void DeleteMyAccount()
        {
            CancelAllCourses(LoggedUser!);
            CancelExams(LoggedUser!);
            studentDAO.DeleteStudent(LoggedUser!.Email);
        }
    

        public void ApplyForCourse(Student student, string courseId)
        {
            student.AddCourse(courseId);
            Course? course = courseService.GetCourseById(courseId);
            course!.AddAttendance();
            courseService.UpdateCourse(course);
        }

        public void CancelAllCourses(Student student)
        {
            foreach (string courseID in student.GetAppliedCourses())
            {
                Course? course = courseService.GetCourseById(courseID);
                course!.CancelAttendance();
                courseService.UpdateCourse(course);
            }
        }

        public void CancelCourse(Student student)
        {
            student.CancelAttendingCourse();
            Course? course = courseService.GetCourseById(student.AttendingCourse);
            course!.CancelAttendance();
            courseService.UpdateCourse(course);
        }


        public void CancelCourseApplication(Student student, string courseID)
        {
            student.CancelCourseApplication(courseID);
        }


        private void CancelExams(Student student)
        {
            foreach (string examID in LoggedUser!.GetAppliedExams())
            {
                Exam? exam = examService.GetExamById(examID);
                exam?.CancelAttendance();
                //examService.UpdateExam(exam!);
                //TODO handle exam updating
            }
        }


    }

}
