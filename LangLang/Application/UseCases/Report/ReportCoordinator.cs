using LangLang.Application.DTO;
using LangLang.Application.Utility.Email;
using System.Collections.Generic;
using LangLang.Application.Utility.PDF;

namespace LangLang.Application.UseCases.Report;

public class ReportCoordinator: IReportCoordinator
{
    private readonly IEmailService _emailService;
    private readonly IReportService _reportService;
    private readonly IPDFReportService _pdfReportService;
    public ReportCoordinator(IEmailService emailService, IReportService reportService, IPDFReportService pdfReportService) 
    { 
        _emailService = emailService;
        _reportService = reportService;
        _pdfReportService = pdfReportService;
    }


    public void SendCoursePenaltyReport(string recipient)
    {
        string emailSubject = "Report for penalty points per course";
        string emailBody = "You've requested a report about the number of penalty points awarded by course. You can find said reported attached bellow.";
        string pdfName = "LangLang Report";
        List<string> reportTitles = new List<string>() { "Penalty points per course", "Average grades per penalty point" };

        List<ReportTableDto> tables = _reportService.GetCoursePenaltyReport();
        _emailService.SendEmailWithPDFAttachment(recipient, emailSubject, emailBody, pdfName, _pdfReportService.GetReportPDF(reportTitles, tables));
    }
    public void SendPointsBySkillReport(string recepient)
    {
        throw new System.NotImplementedException();
    }


    public void SendAverageCoursePointsReport(string recepient)
    {
        string emailSubject = "Report for penalty points per course";
        string emailBody = "You've requested a report about the number of penalty points awarded by course. You can find said reported attached bellow.";
        string pdfName = "LangLang Report";

        List<string> reportTitles = new List<string>() { "Penalty points per course", "Average grades per penalty point" };

        List<ReportTableDto> tables = _reportService.GetAverageCoursePointsReport();
        _emailService.SendEmailWithPDFAttachment(recepient, emailSubject, emailBody, pdfName, _pdfReportService.GetReportPDF(reportTitles,tables));
    }

}
