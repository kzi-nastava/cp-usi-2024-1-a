using LangLang.Application.DTO;
using System.Collections.Generic;

namespace LangLang.Application.UseCases.Report;

public interface IReportService
{
    public List<ReportTableDto> GetCoursePenaltyReport();

}
