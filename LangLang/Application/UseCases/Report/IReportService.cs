using LangLang.Application.DTO;

namespace LangLang.Application.UseCases.Report;

public interface IReportService
{
    public ReportTableDto GetCoursePenaltyReport();

}
