using LangLang.Application.DTO;
using LangLang.Application.UseCases.Course;
using LangLang.Domain.RepositoryInterfaces;
using System.Collections.Generic;


namespace LangLang.Application.UseCases.Report
{
    public class PenaltyByCourseReportService:IPenaltyByCourseReportService
    {
        private readonly ICourseService _courseService;
        private readonly IStudentCourseCoordinator _courseCoordinator;
        private readonly ICourseAttendanceService _courseAttendanceService;
        private readonly IStudentRepository _studentRepository;

        public PenaltyByCourseReportService(ICourseService courseService, IStudentCourseCoordinator courseCoordinator, ICourseAttendanceService attendanceService, IStudentRepository studentRepository)
        {
            _courseService = courseService;
            _courseCoordinator = courseCoordinator;
            _courseAttendanceService = attendanceService;
            _studentRepository = studentRepository;
        }

        public List<ReportTableDto> GetReport()
        {
            return new List<ReportTableDto>
            {
                ConvertPenaltyByCourseDictionaryToReportTable(GetPenaltyByCourseDictionary()),
                ConvertAverageGradesByPenaltyDictionaryToReportTable(GetAverageGradesByPenaltyDictionary())
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

        private static ReportTableDto ConvertAverageGradesByPenaltyDictionaryToReportTable(Dictionary<uint, GradeReport> gradesByPenalty)
        {
            List<List<string>> tableRows = new List<List<string>>();
            foreach (var penaltyPoint in gradesByPenalty.Keys)
            {
                tableRows.Add(new() { penaltyPoint.ToString(), gradesByPenalty[penaltyPoint].AverageActivityGrade.ToString(), gradesByPenalty[penaltyPoint].AverageKnowledgeGrade.ToString() });
            }

            return new ReportTableDto(
                new List<string> { "Penalty point", "Average activity grade", "Average knowledge grade" },
                tableRows
            );
        }


        private record GradeReport
        {
            public int KnowledgeGradeSum { get; set; }
            public int ActivityGradeSum { get; set; }
            public int NumberOfStudents { get; set; }
            public float AverageKnowledgeGrade {  get; set; }
            public float AverageActivityGrade { get; set; }
            public GradeReport()
            {
                KnowledgeGradeSum = 0;
                ActivityGradeSum = 0;
                NumberOfStudents = 0;
            }

            public void AddGrades(int knowledgeGrade, int activityGrade)
            {
                KnowledgeGradeSum += knowledgeGrade;
                ActivityGradeSum += activityGrade;
                NumberOfStudents++;
            }

            public void CalculateAverageGrades()
            {
                if (NumberOfStudents > 0)
                {
                    AverageKnowledgeGrade = 1.0f * KnowledgeGradeSum / NumberOfStudents;
                    AverageActivityGrade = 1.0f * ActivityGradeSum / NumberOfStudents;
                }
            }
        }

        private Dictionary<uint, GradeReport> GetAverageGradesByPenaltyDictionary()
        {
            var gradedAttendances = _courseCoordinator.GetGradedAttendancesForLastYear();
            Dictionary<uint, GradeReport> gradesByPenalty = new Dictionary<uint, GradeReport>()
            {
                { 0, new GradeReport() },
                { 1, new GradeReport() },
                { 2, new GradeReport() },
                { 3, new GradeReport() }
            };
            foreach(Domain.Model.CourseAttendance attendance in  gradedAttendances)
            {
                Domain.Model.Student student = _studentRepository.Get(attendance.StudentId)!;
                if (student != null)
                {
                    gradesByPenalty[student.PenaltyPoints].AddGrades(attendance.KnowledgeGrade, attendance.ActivityGrade);
                }
            }

            for(uint i = 0; i <= 3; i++)
            {
                gradesByPenalty[i].CalculateAverageGrades();
            }
            return gradesByPenalty;
        }


    }
}
