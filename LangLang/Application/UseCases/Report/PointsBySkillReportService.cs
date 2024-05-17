using System.Collections.Generic;
using System.Globalization;
using LangLang.Application.DTO;
using LangLang.Application.UseCases.Course;
using LangLang.Application.UseCases.Exam;

namespace LangLang.Application.UseCases.Report;

public class PointsBySkillReportService : IPointsBySkillReportService
{
    private readonly IExamCoordinator _examCoordinator;
    private readonly ICourseService _courseService;
    private readonly IStudentCourseCoordinator _courseCoordinator;

    public PointsBySkillReportService(IExamCoordinator examCoordinator, ICourseService courseService, IStudentCourseCoordinator courseCoordinator)
    {
        _examCoordinator = examCoordinator;
        _courseService = courseService;
        _courseCoordinator = courseCoordinator;
    }

    public List<ReportTableDto> GetReport()
    {
        return new List<ReportTableDto>
        {
            ConvertPointsBySkillDictionaryToReportTable(GetPointsBySkillDictionary()),
            ConvertCourseReportsToReportTable(GetCourseReports())
        };
    }

    private Dictionary<string, float> GetPointsBySkillDictionary()
    {
        var attendances = _examCoordinator.GetGradedAttendancesForLastYear();
        var totalScore = new Dictionary<string, float>
        {
            {"Reading", 0}, {"Writing", 0}, {"Listening", 0}, {"Speaking", 0}
        };

        if (attendances.Count == 0)
            return totalScore;

        foreach (var attendance in attendances)
        {
            totalScore["Reading"] += attendance.Grade?.ReadingScore ?? 0;
            totalScore["Writing"] += attendance.Grade?.WritingScore ?? 0;
            totalScore["Listening"] += attendance.Grade?.ListeningScore ?? 0;
            totalScore["Speaking"] += attendance.Grade?.SpeakingScore ?? 0;
        }

        totalScore["Reading"] /= attendances.Count;
        totalScore["Writing"] /= attendances.Count;
        totalScore["Listening"] /= attendances.Count;
        totalScore["Speaking"] /= attendances.Count;

        return totalScore;
    }

    private record CourseReport(string CourseName)
    {
        public string CourseName { get; set; } = CourseName;
        public int NumAttendees { get; set; }
        public int NumExamPasses { get; set; }
        public float PassPercentage { get; set; }
    }

    private List<CourseReport> GetCourseReports()
    {
        var reports = new List<CourseReport>();
        var courses = _courseService.GetCoursesForLastYear();
        foreach (var course in courses)
        {
            var report = new CourseReport(course.Name);
            var students = _courseCoordinator.GetAttendanceStudentsCourse(course.Id);
            report.NumAttendees = students.Count;
            foreach (var student in students)
            {
                if (student.HasExamKnowledge(course.Language, course.Level))
                    report.NumExamPasses++;
            }
            if(report.NumAttendees > 0)
                report.PassPercentage = 1.0f * report.NumExamPasses / report.NumAttendees;
            reports.Add(report);
        }
        return reports;
    }

    private static ReportTableDto ConvertPointsBySkillDictionaryToReportTable(Dictionary<string, float> scores)
    {
        return new ReportTableDto(
            new List<string> { "Skill", "Average score" },
            new List<List<string>>
            {
                new() { "Reading", scores["Reading"].ToString(CultureInfo.InvariantCulture) },
                new() { "Writing", scores["Writing"].ToString(CultureInfo.InvariantCulture) },
                new() { "Listening", scores["Listening"].ToString(CultureInfo.InvariantCulture) },
                new() { "Speaking", scores["Speaking"].ToString(CultureInfo.InvariantCulture) }
            }
        );
    }

    private static ReportTableDto ConvertCourseReportsToReportTable(List<CourseReport> courseReports)
    {
        var rows = new List<List<string>>();
        foreach (var report in courseReports)
        {
            var numAttendees = report.NumAttendees.ToString();
            var numExamPasses = report.NumExamPasses.ToString();
            var passPercentage = (report.PassPercentage * 100).ToString("0.00");
            rows.Add(new List<string> {report.CourseName, numAttendees, numExamPasses, passPercentage});
        }

        return new ReportTableDto(
            new List<string> { "Course name", "# attendees", "# exam passes", "pass percentage" },
            rows
        );
    }
}