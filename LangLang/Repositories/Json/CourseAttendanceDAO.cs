using System.Collections.Generic;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;
using LangLang.Repositories.Json.Util;

namespace LangLang.Repositories.Json
{
    public class CourseAttendanceDAO: ICourseAttendanceDAO
    {
        private readonly ILastIdDAO _lastIdDAO;

        public CourseAttendanceDAO(ILastIdDAO lastIdDAO)
        {
            _lastIdDAO = lastIdDAO;
        }
        public Dictionary<string, CourseAttendance> GetAllCourseAttendances()
        {
            return JsonUtil.ReadFromFile<CourseAttendance>(Constants.CourseAttendancesFilePath);
        }
        public CourseAttendance? GetCourseAttendanceById(string id)
        {
            Dictionary<string, CourseAttendance> courseAttendances = GetAllCourseAttendances();
            return courseAttendances.GetValueOrDefault(id);
        }
        
        public List<CourseAttendance> GetCourseAttendancesForCourse(string courseId)
        {
            List<CourseAttendance> attendances = new();
            foreach (CourseAttendance attendance in GetAllCourseAttendances().Values)
            {
                if (attendance.CourseId == courseId)
                {
                    attendances.Add(attendance);
                }
            }
            return attendances;
        }
        
        public List<CourseAttendance> GetAllCourseAttendancesForStudent(string studentId)
        {
            List<CourseAttendance> attendances = new();
            foreach (CourseAttendance attendance in GetAllCourseAttendances().Values)
            {
                if (attendance.StudentId == studentId)
                {
                    attendances.Add(attendance);
                }
            }
            return attendances;
        }

        public CourseAttendance AddCourseAttendance(CourseAttendance attendance)
        {
            _lastIdDAO.IncrementCourseAttendanceId();
            attendance.Id = _lastIdDAO.GetCourseAttendanceId();
            Dictionary<string, CourseAttendance> courseAttendances = GetAllCourseAttendances();
            courseAttendances.Add(attendance.Id, attendance);
            SaveCourseAttendances(courseAttendances);
            return attendance;
        }
        
        public void DeleteCourseAttendance(string id)
        {
            Dictionary<string, CourseAttendance> courseAttendances = GetAllCourseAttendances();
            courseAttendances.Remove(id);
            SaveCourseAttendances(courseAttendances);
        }
        
        public CourseAttendance? UpdateCourseAttendance(string id, CourseAttendance attendance)
        {
            Dictionary<string, CourseAttendance> courseAttendances = GetAllCourseAttendances();
            if (!courseAttendances.ContainsKey(id)) return null;
            courseAttendances[id] = attendance;
            SaveCourseAttendances(courseAttendances);
            return attendance;
        }
        
        private void SaveCourseAttendances(Dictionary<string, CourseAttendance> courseAttendances)
        {
            JsonUtil.WriteToFile(courseAttendances, Constants.CourseAttendancesFilePath);
        }



    }
}
