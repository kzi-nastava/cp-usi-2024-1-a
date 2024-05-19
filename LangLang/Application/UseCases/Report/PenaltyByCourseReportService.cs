using LangLang.Application.DTO;
using LangLang.Application.UseCases.Course;
using System.Collections.Generic;


namespace LangLang.Application.UseCases.Report
{
    public class PenaltyByCourseReportService:IPenaltyByCourseReportService
    {
        private readonly ICourseService _courseService;
        private readonly IStudentCourseCoordinator _courseCoordinator;
        private readonly ICourseAttendanceService _courseAttendanceService;

        public PenaltyByCourseReportService(ICourseService courseService, IStudentCourseCoordinator courseCoordinator, ICourseAttendanceService attendanceService)
        {
            _courseService = courseService;
            _courseCoordinator = courseCoordinator;
            _courseAttendanceService = attendanceService;
        }

        public List<ReportTableDto> GetReport()
        {
            return new List<ReportTableDto>
            {
                ConvertPenaltyByCourseDictionaryToReportTable(GetPenaltyByCourseDictionary())
            };
        }


        public Dictionary<string, int> GetPenaltyByCourseDictionary() {
            Dictionary<string, int> penaltyByCourse = new Dictionary<string, int>();
            var courses = _courseService.GetCoursesForLastYear();
            foreach(Domain.Model.Course course in courses) {
                var attendances = _courseAttendanceService.GetAttendancesForCourse(course.Id);
                penaltyByCourse[course.Name] = 0;
                foreach (var attendance in attendances) {
                    penaltyByCourse[course.Name] += attendance.PenaltyPoints;
                }
            }
        
            return penaltyByCourse;
        }


        private static ReportTableDto ConvertPenaltyByCourseDictionaryToReportTable(Dictionary<string, int> penaltyByCourse)
        {
            List<List<string>> tableRows = new List<List<string>>();
            foreach (var courseName in penaltyByCourse.Keys)
            {
                tableRows.Add(new() { courseName, penaltyByCourse[courseName].ToString() });
            }

            return new ReportTableDto(
                new List<string> { "Course", "Penalty points awarded" },
                tableRows
            );

        }

    }
}
