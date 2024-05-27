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

    private readonly string[] _coursesHeader = new string[] { "Language", "Courses created", "Penalty points", "Activity grade", "Knowledge grade" };
    private readonly string[] _examsHeader = new string[] { "Language", "Exams created", "Reading", "Writing", "Listening", "Speaking" };

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
        foreach (var couse in courses)
        {
            if (!created.ContainsKey(couse.Language))
                created[couse.Language] = 0;
            created[couse.Language]++;
        }

        var attendences = _courseCoordinator.GetGradedAttendancesForLastYear();
        foreach (var attendance in attendences)
        {
            Language language = _courseService.GetCourseById(attendance.CourseId)!.Language;
            if (!totalPenaltyPoints.ContainsKey(language))
            {
                totalPenaltyPoints [language] = 0;
                totalActivityGrade [language] = 0;
                totalKnowledgeGrade[language] = 0;
                counts[language] = 0;
            }
            totalPenaltyPoints [language] += attendance.PenaltyPoints;
            totalActivityGrade [language] += attendance.ActivityGrade;
            totalKnowledgeGrade[language] += attendance.KnowledgeGrade;
            counts[language]++;
        }
        List<List<string>> rows = new();
        foreach(Language language in created.Keys)
        {
            List<string> row = new() { 
                language.Name,                  // language
                created[language].ToString() }; // courses created
            if (totalPenaltyPoints.ContainsKey(language)) { // created keys are always a superset of others' keys
                uint count = counts[language];
                row.AddRange(new List<string>
                {
                    (totalPenaltyPoints [language]/count).ToString(CultureInfo.InvariantCulture), // average penalty points
                    (totalActivityGrade [language]/count).ToString(CultureInfo.InvariantCulture), // average activity grade
                    (totalKnowledgeGrade[language]/count).ToString(CultureInfo.InvariantCulture)  // average knoweldge grade
                });
            }
            else
                row.AddRange(new List<string> { "N/A", "N/A", "N/A" });
            rows.Add(row);
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
        {
            if (!created.ContainsKey(exam.Language))
                created[exam.Language] = 0;
            created[exam.Language]++;
        }

        var attendences = _examCoordinator.GetGradedAttendancesForLastYear();
        foreach (var attendance in attendences)
        {
            Language language = _examService.GetExamById(attendance.ExamId)!.Language;
            ExamGrade grade = attendance.Grade!; // we believe that GetGradedAttendancesForLastYear returned only graded exams
            if (!totalReading.ContainsKey(language))
            {
                totalReading  [language] = 0;
                totalWriting  [language] = 0;
                totalListening[language] = 0;
                totalSpeaking [language] = 0;
                counts        [language] = 0;
            }
            totalReading  [language] += grade.ReadingScore;
            totalWriting  [language] += grade.WritingScore;
            totalListening[language] += grade.ListeningScore;
            totalSpeaking [language] += grade.SpeakingScore;
            counts[language]++;
        }
        List<List<string>> rows = new();
        foreach (Language language in created.Keys)
        {
            List<string> row = new() {
                language.Name,                  // language
                created[language].ToString() }; // exams created
            if (totalReading.ContainsKey(language)) { // created keys are always a superset of others' keys
                uint count = counts[language];
                row.AddRange(new List<string>
                {
                    (totalReading  [language]/count).ToString(CultureInfo.InvariantCulture), // average reading
                    (totalWriting  [language]/count).ToString(CultureInfo.InvariantCulture), // average writing
                    (totalListening[language]/count).ToString(CultureInfo.InvariantCulture), // average listening
                    (totalSpeaking [language]/count).ToString(CultureInfo.InvariantCulture)  // average speaking
                });
            }
            else
                row.AddRange(new List<string> { "N/A", "N/A", "N/A", "N/A" });
            rows.Add(row);
        }

        return new ReportTableDto(_examsHeader.ToList(), rows);
    }
}