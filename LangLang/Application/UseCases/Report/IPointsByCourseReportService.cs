using LangLang.Application.DTO;
using System.Collections.Generic;

namespace LangLang.Application.UseCases.Report;

public interface IPointsByCourseReportService
{
    public List<ReportTableDto> GetReport();
}
