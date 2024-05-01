using LangLang.Model;
using LangLang.Util;
using Consts;
using System.Collections.Generic;

namespace LangLang.DAO.JsonDao
{
    public class CourseAttendanceDAO: ICourseAttendanceDAO
    {
        private Dictionary<string, CourseAttendance>? _courseAttendance;
        private readonly ILastIdDAO _lastIdDAO;

        private Dictionary<string, CourseAttendance> CourseAttendances
        {
            get
            {
                _courseAttendance ??= JsonUtil.ReadFromFile<CourseAttendance>(Constants.CourseAttendancesFilePath);
                return _courseAttendance;
            }
            set
            {
                _courseAttendance = value;
            }
        }

        public CourseAttendanceDAO(ILastIdDAO lastIdDAO)
        {
            _lastIdDAO = lastIdDAO;
        }
        public Dictionary<string, CourseAttendance> GetAllCourseAttendances()
        {
            return CourseAttendances;
        }
        public CourseAttendance? GetCourseAttendanceById(string id)
        {
            return CourseAttendances.GetValueOrDefault(id);
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
        public List<CourseAttendance> GetCourseAttendancesForStudent(string studentId)
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
        public CourseAttendance? GetStudentAttendanceForCourse(string studentId, string courseId)
        {
            List<CourseAttendance> attendances = GetCourseAttendancesForStudent(studentId);
            foreach (CourseAttendance attendance in attendances)
            {
                if (attendance.CourseId == courseId)
                {
                    return attendance;
                }
            }
            return null;
        }

        public CourseAttendance AddCourseAttendance(CourseAttendance attendance)
        {
            _lastIdDAO.IncrementCourseAttendanceId();
            attendance.Id = _lastIdDAO.GetCourseAttendanceId();
            CourseAttendances.Add(attendance.Id, attendance);
            SaveCourseAttendances();
            return attendance;
        }
        public void DeleteCourseAttendance(string id)
        {
            CourseAttendances.Remove(id);
            SaveCourseAttendances();
        }
        public CourseAttendance? UpdateCourseAttendance(string id, CourseAttendance attendance)
        {
            if (!CourseAttendances.ContainsKey(id)) return null;
            CourseAttendances[id] = attendance;
            return attendance;
        }
        private void SaveCourseAttendances()
        {
            JsonUtil.WriteToFile(CourseAttendances, Constants.CourseAttendancesFilePath);
        }



    }
}
