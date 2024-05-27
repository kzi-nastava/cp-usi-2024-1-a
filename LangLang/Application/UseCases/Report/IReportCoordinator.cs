namespace LangLang.Application.UseCases.Report;

public interface IReportCoordinator
{
    public void SendCoursePenaltyReport(string recipient);
    public void SendLanguageReport(string recipient);
}
