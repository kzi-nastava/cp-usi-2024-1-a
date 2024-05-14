using System.Collections.Generic;
using LangLang.Domain.Model;

namespace LangLang.Domain.RepositoryInterfaces;
public interface IExamAttendanceDAO
{
    public Dictionary<string, ExamAttendance> GetAllExamAttendances();
    public ExamAttendance? GetExamAttendanceById(string id);
    public List<ExamAttendance> GetExamAttendancesForExam(string examId);
    public List<ExamAttendance> GetExamAttendancesForStudent(string studentId);
    public ExamAttendance? GetStudentAttendanceForExam(string studentId, string examId);
    public ExamAttendance AddExamAttendance(ExamAttendance attendance);
    public ExamAttendance? UpdateExamAttendance(string id, ExamAttendance attendance);
    public void DeleteExamAttendance(string id);


}

