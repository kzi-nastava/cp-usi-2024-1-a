using LangLang.Application.DTO;
using System.Collections.Generic;


namespace LangLang.Application.UseCases.Report;

public interface IPenaltyByCourseReportService
{
    public List<ReportTableDto> GetReport();
}
