using LangLang.Application.UseCases.Course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Application.UseCases.Report;

public class ReportService
{
    private readonly ICourseAttendanceService _courseAttendanceService;
    public ReportService(ICourseAttendanceService courseAttendanceService)
    {
        _courseAttendanceService = courseAttendanceService;
    }

    public List<List<string>> GetCoursePenaltyReport()
    {
        
    }


}
