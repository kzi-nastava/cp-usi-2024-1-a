using LangLang.Domain.Model;


namespace LangLang.Application.UseCases.Report;

public interface IReportService
{
    public ReportTableData GetCoursePenaltyReport();

}
