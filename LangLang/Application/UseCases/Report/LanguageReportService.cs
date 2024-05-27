using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using LangLang.Application.DTO;
using LangLang.Application.UseCases.Common;
using LangLang.Application.UseCases.Course;
using LangLang.Application.UseCases.Exam;
using LangLang.Domain.Model;

namespace LangLang.Application.UseCases.Report;

public class LanguageReportService : ILanguageReportService
{
    private readonly IExamService _examService;
    private readonly IExamCoordinator _examCoordinator;
    private readonly ICourseService _courseService;
    private readonly IStudentCourseCoordinator _courseCoordinator;

    private readonly string[] _coursesHeader = new string[] { "Courses created", "Average penalty points", "Average activity grade", "Average knowledge grade" };
    private readonly string[] _examsHeader = new string[] { "Exams created", "Average reading", "Average writing", "Average listening", "Average speaking" };

    public LanguageReportService(IExamCoordinator examCoordinator, ICourseService courseService, IStudentCourseCoordinator courseCoordinator, IExamService examService)
    {
        _examCoordinator = examCoordinator;
        _courseService = courseService;
        _courseCoordinator = courseCoordinator;
        _examService = examService;
    }

    public List<ReportTableDto> GetReport()
    {
        return new List<ReportTableDto>
        {
            CreateCourseReport(),
            CreateExamReport()
        };
    }

    private ReportTableDto CreateCourseReport()
    {
        Dictionary<Language, uint> created = new();
        Dictionary<Language, int> totalPenaltyPoints = new();
        Dictionary<Language, int> totalActivityGrade = new();
        Dictionary<Language, int> totalKnowledgeGrade = new();
        Dictionary<Language, uint> counts = new();

        var courses = _courseService.GetCoursesForLastYear();
        foreach (var course in courses)
            created.Add(course.Language, 1);
         
        var attendences = _courseCoordinator.GetGradedAttendancesForLastYear();
        foreach (var attendance in attendences)
        {
            Language language = _courseService.GetCourseById(attendance.CourseId)!.Language;
            totalPenaltyPoints.Add(language, attendance.PenaltyPoints);
            totalActivityGrade.Add(language, attendance.ActivityGrade);
            totalKnowledgeGrade.Add(language, attendance.KnowledgeGrade);
            counts.Add(language, 1);
        }
        List<List<string>> rows = new();
        foreach(Language language in created.Keys)
        {
            List<string> row = new() { created[language].ToString() }; // courses created
            if (totalPenaltyPoints.ContainsKey(language)) { // created keys are always a superset of others' keys
                uint count = counts[language];
                row.AddRange(new List<string>
                {
                    (totalPenaltyPoints [language]/count).ToString(CultureInfo.InvariantCulture), // average penalty points
                    (totalActivityGrade [language]/count).ToString(CultureInfo.InvariantCulture), // average activity grade
                    (totalKnowledgeGrade[language]/count).ToString(CultureInfo.InvariantCulture)  // average knoweldge grade
                });
            }
        }

        return new ReportTableDto(_coursesHeader.ToList(), rows);
    }
    private ReportTableDto CreateExamReport()
    {
        Dictionary<Language, uint> created = new();
        Dictionary<Language, uint> totalReading   = new();
        Dictionary<Language, uint> totalWriting   = new();
        Dictionary<Language, uint> totalListening = new();
        Dictionary<Language, uint> totalSpeaking  = new();
        Dictionary<Language, uint> counts = new();

        var exams = _examService.GetExamsForTimePeriod(DateTime.Now.AddYears(-1), DateTime.Now);
        foreach (var exam in exams)
            created.Add(exam.Language, 1);

        var attendences = _examCoordinator.GetGradedAttendancesForLastYear();
        foreach (var attendance in attendences)
        {
            Language language = _examService.GetExamById(attendance.ExamId)!.Language;
            ExamGrade grade = attendance.Grade!; // we believe that GetGradedAttendancesForLastYear returned only graded exams
            totalReading.Add(language, grade.ReadingScore);
            totalWriting.Add(language, grade.WritingScore);
            totalListening.Add(language, grade.ListeningScore);
            totalSpeaking.Add(language, grade.SpeakingScore);
            counts.Add(language, 1);
        }
        List<List<string>> rows = new();
        foreach (Language language in created.Keys)
        {
            List<string> row = new() { created[language].ToString() }; // exams created
            if (totalReading.ContainsKey(language)){ // created keys are always a superset of others' keys
                uint count = counts[language];
                row.AddRange(new List<string>
                {
                    (totalReading  [language]/count).ToString(CultureInfo.InvariantCulture), // average reading
                    (totalWriting  [language]/count).ToString(CultureInfo.InvariantCulture), // average writing
                    (totalListening[language]/count).ToString(CultureInfo.InvariantCulture), // average listening
                    (totalSpeaking [language]/count).ToString(CultureInfo.InvariantCulture)  // average speaking
                });
            }
        }

        return new ReportTableDto(_examsHeader.ToList(), rows);
    }
}