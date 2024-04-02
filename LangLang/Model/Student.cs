using Consts;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

namespace LangLang.Model
{
    public class Student : User
    {
        //---------------------------Languages
        private List<string> coursesApplied;    //string is courseId
        private List<string> examsApplied;
        private List<string> notifications;


        //--------------------------Properties
        public string Qualification { get; set; }
        public uint PenaltyPts { get; set; }
        public string AttendingCourse { get; set; }
        public string AttendingExam {  get; set; }

        public Student() : base("", "", "", "", DateTime.Now, Gender.Other, "")
        {
            PenaltyPts = 0;
            AttendingCourse = "";
            AttendingExam = "";
            Qualification = "";
            this.coursesApplied = new List<string>();
            this.examsApplied = new List<string>();
            this.notifications = new List<string>();
        }

        public Student(string email, string password, string name, string surname, DateTime birthDate, Gender gender, string phoneNumber, string qualification, uint penaltyPts, string attendingExam, string attendingCourse, List<string> coursesApplied, List<string> examsApplied, List<string> notifications)
            : base(email, password, name, surname, birthDate, gender, phoneNumber)
        {
            PenaltyPts = penaltyPts;
            AttendingCourse = attendingCourse;
            AttendingExam = attendingExam;
            Qualification = qualification;
            this.coursesApplied = coursesApplied;
            this.examsApplied = examsApplied;
            this.notifications = notifications;
        }

        public void RemovePenaltyPts()
        {
            if (PenaltyPts > 0) { PenaltyPts--; }
        }

        public void AddCourse(string course)
        {
            coursesApplied.Add(course);
        }

        public void AddExam(string exam)
        {
            examsApplied.Add(exam);
        }

        public List<string> GetAppliedCourses()
        {
            return coursesApplied;
        }

        public List<string> GetAppliedExams()
        {
            return examsApplied;
        }


        public void AddNotification(string notification)
        {
            notifications.Add(notification);
        }

        public List<string> GetNotifications()
        {
            return notifications;
        }
    }
}

