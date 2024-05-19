using LangLang.Application.DTO;
using LangLang.Application.UseCases.Course;
using LangLang.Application.UseCases.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Application.UseCases.Report
{
    public class PenaltyByCourseReportService:IPenaltyByCourseReportService
    {
        private readonly ICourseService _courseService;
        private readonly IStudentCourseCoordinator _courseCoordinator;

        public PenaltyByCourseReportService(ICourseService courseService, IStudentCourseCoordinator courseCoordinator)
        {
            _courseService = courseService;
            _courseCoordinator = courseCoordinator;
        }

        public List<ReportTableDto> GetReport()
        {
            return new List<ReportTableDto>
            {
                ConvertPointsBySkillDictionaryToReportTable(GetPenaltyByCourseDictionary()),
                ConvertCourseReportsToReportTable(GetCourseReports())
            };
        }


        public Dictionary<string, int> GetPenaltyByCourseDictionary() {
            var courses = _courseService.GetCoursesForLastYear();
            foreach(Domain.Model.Course course in courses) {
                var attendances = _courseCoordinator.Get
            }
        
        
        
        
        }
    }
}
