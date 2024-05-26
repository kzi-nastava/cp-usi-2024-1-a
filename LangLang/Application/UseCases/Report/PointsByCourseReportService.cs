using LangLang.Application.DTO;
using LangLang.Application.UseCases.Course;
using LangLang.Application.UseCases.Exam;
using LangLang.Application.UseCases.User;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Application.UseCases.Report;

public class PointsByCourseReportService : IPointsByCourseReportService
{
    private readonly ICourseService _courseService;
    private readonly IStudentCourseCoordinator _studentCourseCoordinator;
    private readonly ITutorService _tutorService;
    public PointsByCourseReportService(ICourseService courseService, ITutorService tutotService, IStudentCourseCoordinator studentCourseCoordinator)
    {
        _studentCourseCoordinator = studentCourseCoordinator;
        _courseService = courseService;
        _tutorService = tutotService;
    }
    public List<ReportTableDto> GetReport()
    {
        return new List<ReportTableDto>
        {
            ConvertPointsDictionaryToReportTable(GetPointsPerCourseDictionary()),
            ConvertCourseReportsToReportTable(GetCourseReports())
        };
    }
    private record TutorReport(string TutorName)
    {
        public string TutorName { get; set; } = TutorName;
        public double AveragePoints { get; set; }
    }

    private List<TutorReport> GetCourseReports()
    {
        var reports = new List<TutorReport>();
        var tutors = _tutorService.GetAllTutors();
        foreach (var tutor in tutors)
        {
            var report = new TutorReport(tutor.Name);
            report.AveragePoints = tutor.GetAverageRating();
            reports.Add(report);
        }
        return reports;
    }

    private ReportTableDto ConvertCourseReportsToReportTable(List<TutorReport> reports)
    {
        var rows = new List<List<string>>();
        foreach(var report in reports)
        {
            var averageScore = report.AveragePoints.ToString();
            rows.Add(new List<string> { report.TutorName, averageScore });
        }
        return new ReportTableDto
        (
            new List<string> { "Tutor name", "average score" },
            rows
        );
    }

    private Dictionary<string, Dictionary<string, float>> GetPointsPerCourseDictionary()
    {
        var coursesScore = new Dictionary<string, Dictionary<string, float>>();

        var attendancePerCourse = new Dictionary<string, int>();

        var attendances = _studentCourseCoordinator.GetGradedAttendancesForLastYear();
        var courses = _courseService.GetAll();
        foreach(var course in courses)
        {
            coursesScore.Add(course.Id, new Dictionary<string, float> { { "Knowledge", 0 }, { "Activity" , 0 }, });
            attendancePerCourse.Add(course.Id, 0);
        }
        if (attendances.Count == 0)
        {
            return coursesScore;
        }

        foreach(var attendance in attendances)
        {
            coursesScore[attendance.CourseId]["Activity"] += attendance.ActivityGrade;
            coursesScore[attendance.CourseId]["Knowledge"] += attendance.KnowledgeGrade;

            attendancePerCourse[attendance.CourseId] += 1;
        }

        foreach(KeyValuePair<string, int> pair in attendancePerCourse)
        {
            coursesScore[pair.Key]["Activity"] /= pair.Value;
            coursesScore[pair.Key]["Knowledge"] /= pair.Value;
        }

        return coursesScore;
    }

    private ReportTableDto ConvertPointsDictionaryToReportTable(Dictionary<string, Dictionary<string, float>> scores)
    {
        var scoresPrint = new List<List<string>>();
        foreach(KeyValuePair<string, Dictionary<string, float>> courseScore in scores)
        {
            scoresPrint.Add(new() { courseScore.Key, courseScore.Value["Activity"].ToString(CultureInfo.InvariantCulture),
                courseScore.Value["Knowledge"].ToString(CultureInfo.InvariantCulture) });
        }


        return new ReportTableDto(
            new List<string> { "Course id", "Avgerage activity", "Average knowledge" },
            scoresPrint

            );
    }
}
