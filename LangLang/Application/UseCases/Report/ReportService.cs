using LangLang.Application.DTO;
using System.Collections.Generic;


namespace LangLang.Application.UseCases.Report;

public class ReportService: IReportService
{
    private readonly IPointsBySkillReportService _pointsBySkillReportService;
    private readonly IPenaltyByCourseReportService _penaltyByCourseReportService;
    private readonly IPointsByCourseReportService _pointsByCourseReportService;

    public ReportService(IPointsBySkillReportService pointsBySkillReportService, IPenaltyByCourseReportService penaltyByCourseReportService, IPointsByCourseReportService pointsByCourseReportService)
    {
        _pointsBySkillReportService = pointsBySkillReportService;
        _penaltyByCourseReportService = penaltyByCourseReportService;
        _pointsByCourseReportService = pointsByCourseReportService;
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
        return _pointsByCourseReportService.GetReport();
    }
}
