﻿using LangLang.DAO;
using LangLang.Model;
using LangLang.Services.CourseServices;
using LangLang.Services.UserServices;
using System.Collections.Generic;


namespace LangLang.Services.StudentCourseServices
{
    public class CourseAttendanceService : ICourseAttendanceService
    {
        private readonly ICourseService _courseService;
        private readonly IStudentService _studentService;
        private readonly ITutorService _tutorService;
        private readonly ICourseAttendanceDAO _courseAttendanceDAO;
        private readonly ILastIdDAO _lastIdDAO;

        public CourseAttendanceService(ICourseService courseService, IStudentService studentService, ITutorService tutorService, ICourseAttendanceDAO courseAttendanceDAO, ILastIdDAO lastIdDAO)
        {
            _courseService = courseService;
            _studentService = studentService;
            _tutorService = tutorService;
            _courseAttendanceDAO = courseAttendanceDAO;
            _lastIdDAO = lastIdDAO;
        }

        public List<CourseAttendance> GetAttendancesForStudent(string studentId)
        {
            return _courseAttendanceDAO.GetCourseAttendancesForStudent(studentId);
        }

        public List<CourseAttendance> GetAttendancesForCourse(string courseId)
        {
            return _courseAttendanceDAO.GetCourseAttendancesForCourse(courseId);
        }

        public CourseAttendance? GetStudentAttendance(string studentId)
        {
            List<CourseAttendance> attendances = _courseAttendanceDAO.GetCourseAttendancesForStudent(studentId);
            foreach (CourseAttendance attendance in attendances)
            {
                Course course = _courseService.GetCourseById(attendance.CourseId)!;
                if (course.State == Consts.CourseState.Active)
                {
                    return attendance;
                }
            }
            return null;
        }
        public List<CourseAttendance> GetFinishedCoursesStudent(string studentId)
        {
            List<CourseAttendance> attendances = _courseAttendanceDAO.GetCourseAttendancesForStudent(studentId);
            foreach (CourseAttendance attendance in attendances)
            {
                Course course = _courseService.GetCourseById(attendance.CourseId)!;
                if (course.State == Consts.CourseState.Finished)
                    attendances.Add(attendance);
            }
            return attendances;
        }

        public CourseAttendance AddAttendance(string studentId, string courseId)
        {
            _lastIdDAO.IncrementCourseAttendanceId();
            string id = _lastIdDAO.GetCourseAttendanceId();
            CourseAttendance attendance = new CourseAttendance(id, courseId, studentId, false, false);
            _courseAttendanceDAO.AddCourseAttendance(attendance);
            return attendance;
        }

        public void RemoveAttendee(string studentId, string courseId)
        {
            List<CourseAttendance> attendances = _courseAttendanceDAO.GetCourseAttendancesForStudent(studentId);
            foreach (CourseAttendance attendance in attendances)
            {
                if(attendance.CourseId == courseId && _courseService.GetCourseById(courseId)!.State == Consts.CourseState.Active)
                    _courseAttendanceDAO.DeleteCourseAttendance(attendance.Id);
            }
        }

        public void GradeStudent(string studentId, string CourseId, int knowledgeGrade, int activityGrade)
        {
            //predavac 6., i dont see this being used anywhere later on
        }

        public void RateTutor(CourseAttendance attendance, int rating)
        {
            if (!attendance.isRated)
            {
                attendance.AddRating();
                Course course = _courseService.GetCourseById(attendance.CourseId)!;
                //Tutor tutor = _tutorService.GetTutor(course.TutorId);
                Tutor tutor = _tutorService.GetTutorForCourse(course.Id)!;
               _tutorService.AddRating(tutor, rating);  //after tutor id gets added to course/exam
                                                        //i will only pass tutor id and then the service will findById
            }
        }


    }
}
