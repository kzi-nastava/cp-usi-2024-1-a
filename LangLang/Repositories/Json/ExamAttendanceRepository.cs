using System.Collections.Generic;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;

namespace LangLang.Repositories.Json
{
    public class ExamAttendanceRepository: AutoIdRepository<ExamAttendance>, IExamAttendanceRepository
    {
        public ExamAttendanceRepository(string filepath, string lastIdFilePath) : base(filepath, lastIdFilePath)
        {
        }
        
        public List<ExamAttendance> GetForExam(string examId)
        {
            List<ExamAttendance> attendances = new();
            foreach (ExamAttendance attendance in GetAll())
            {
                if (attendance.ExamId == examId)
                {
                    attendances.Add(attendance);
                }
            }
            return attendances;
        }
        public List<ExamAttendance> GetForStudent(string studentId)
        {
            List<ExamAttendance> attendances = new();
            foreach (ExamAttendance attendance in GetAll())
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
            List<ExamAttendance> attendances = GetForStudent(studentId);
            foreach (ExamAttendance attendance in attendances)
            {
                if (attendance.ExamId == examId)
                {
                    return attendance;
                }
            }
            return null;
        }
    }
}
