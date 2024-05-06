using LangLang.Model;
using LangLang.Util;
using Consts;
using System.Collections.Generic;

namespace LangLang.DAO.JsonDao
{
    public class ExamAttendanceDAO: IExamAttendanceDAO
    {
        private Dictionary<string, ExamAttendance>? _examAttendance;
        private readonly ILastIdDAO _lastIdDAO;

        private Dictionary<string, ExamAttendance> ExamAttendances
        {
            get
            {
                _examAttendance ??= JsonUtil.ReadFromFile<ExamAttendance>(Constants.ExamAttendancesFilePath);
                return _examAttendance;
            }
            set
            {
                _examAttendance = value;
            }
        }

        public ExamAttendanceDAO(ILastIdDAO lastIdDAO)
        {
            _lastIdDAO = lastIdDAO;
        }
        public Dictionary<string, ExamAttendance> GetAllExamAttendances()
        {
            return ExamAttendances;
        }
        public ExamAttendance? GetExamAttendanceById(string id)
        {
            return ExamAttendances.GetValueOrDefault(id);
        }
        public List<ExamAttendance> GetExamAttendancesForExam(string examId)
        {
            List<ExamAttendance> attendances = new();
            foreach (ExamAttendance attendance in GetAllExamAttendances().Values)
            {
                if (attendance.ExamId == examId)
                {
                    attendances.Add(attendance);
                }
            }
            return attendances;
        }
        public List<ExamAttendance> GetExamAttendancesForStudent(string studentId)
        {
            List<ExamAttendance> attendances = new();
            foreach (ExamAttendance attendance in GetAllExamAttendances().Values)
            {
                if (attendance.StudentId == studentId)
                {
                    attendances.Add(attendance);
                }
            }
            return attendances;
        }
        public ExamAttendance? GetStudentAttendanceForExam(string studentId, string examId)
        {
            List<ExamAttendance> attendances = GetExamAttendancesForStudent(studentId);
            foreach (ExamAttendance attendance in attendances)
            {
                if (attendance.ExamId == examId)
                {
                    return attendance;
                }
            }
            return null;
        }

        public ExamAttendance AddExamAttendance(ExamAttendance attendance)
        {
            _lastIdDAO.IncrementExamAttendanceId();
            attendance.Id = _lastIdDAO.GetExamAttendanceId();
            ExamAttendances.Add(attendance.Id, attendance);
            SaveExamAttendances();
            return attendance;
        }
        public void DeleteExamAttendance(string id)
        {
            ExamAttendances.Remove(id);
            SaveExamAttendances();
        }
        public ExamAttendance? UpdateExamAttendance(string id, ExamAttendance attendance)
        {
            if (!ExamAttendances.ContainsKey(id)) return null;
            ExamAttendances[id] = attendance;
            return attendance;
        }
        private void SaveExamAttendances()
        {
            JsonUtil.WriteToFile(ExamAttendances, Constants.ExamAttendancesFilePath);
        }



    }
}
