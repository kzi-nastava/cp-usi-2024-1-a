using LangLang.Application.DTO;
using System.Collections.Generic;


namespace LangLang.Application.UseCases.Report;

public class ReportService: IReportService
{
    private readonly IPointsBySkillReportService _pointsBySkillReportService;
    private readonly IPenaltyByCourseReportService _penaltyByCourseReportService;

    public ReportService(IPointsBySkillReportService pointsBySkillReportService, IPenaltyByCourseReportService penaltyByCourseReportService)
    {
        _pointsBySkillReportService = pointsBySkillReportService;
        _penaltyByCourseReportService = penaltyByCourseReportService;
    }

    public List<ReportTableDto> GetCoursePenaltyReport()
    {
        return _penaltyByCourseReportService.GetReport();
    }

    public List<ReportTableDto> GetPointsBySkillReport()
    {
        return _pointsBySkillReportService.GetReport();
    }
    public List<ReportTableDto> GetAverageCoursePointsReport()
    {
        
    }
}
