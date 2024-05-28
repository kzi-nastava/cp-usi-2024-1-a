namespace LangLang.Application.UseCases.Report;

public interface IReportCoordinator
{
    public void SendCoursePenaltyReport(string recipient);

    public void SendPointsBySkillReport(string recepient);

    public void SendAverageCoursePointsReport(string recepient);

    public void SendLanguageReport(string recipient);
}
