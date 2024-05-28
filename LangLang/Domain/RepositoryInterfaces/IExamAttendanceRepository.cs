using System.Collections.Generic;
using LangLang.Domain.Model;

namespace LangLang.Domain.RepositoryInterfaces;
public interface IExamAttendanceRepository : IRepository<ExamAttendance>
{
    public List<ExamAttendance> GetForExam(string examId);
    public List<ExamAttendance> GetForStudent(string studentId);
    public ExamAttendance? GetStudentAttendanceForExam(string studentId, string examId);
}

