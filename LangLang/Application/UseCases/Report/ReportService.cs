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
        /*//temporary table data
        List<string> columnNames = new List<string>()
        {
            "name",
            "language",
            "level"
        };
        List<List<string>> list2 = new List<List<string>>();
        for (int i = 0; i < 100; i++)
        {
            list2.Add(new List<string>()
            {
                "marija",
                "parezanin",
                "hey"
            });

        }

        List<List<string>> list3 = new List<List<string>>();
        for (int i = 0; i < 100; i++)
        {
            list3.Add(new List<string>()
            {
                "table",
                "2",
                "hey"
            });

        }
        List<ReportTableDto> tables = new List<ReportTableDto>
        {
            new ReportTableDto(columnNames, list2),
            new ReportTableDto(columnNames, list3)
        };

        return tables;*/
    }

    public List<ReportTableDto> GetPointsBySkillReport()
    {
        return _pointsBySkillReportService.GetReport();
    }
}
