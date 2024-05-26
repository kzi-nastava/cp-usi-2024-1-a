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
        string emailSubject = "Report for average student grades per skill.";
        string emailBody = "You've requested a report about the student scores. Also, in report you can find " +
            "course statistics. You can find said reported attached bellow.";
        string pdfName = "LangLang Report";

        List<string> reportTitles = new List<string>() { "Student grades", "Course statistics" };

        List<ReportTableDto> tables = _reportService.GetPointsBySkillReport();
        _emailService.SendEmailWithPDFAttachment(recepient, emailSubject, emailBody, pdfName, _pdfReportService.GetReportPDF(reportTitles, tables));
    }


    public void SendAverageCoursePointsReport(string recepient)
    {
        string emailSubject = "Report for average student activity and knowledge on courses.";
        string emailBody = "You've requested a report about the average student activity and knowledge. Also, in report you can find " +
            "average tutor scores. You can find said reported attached bellow.";
        string pdfName = "LangLang Report";

        List<string> reportTitles = new List<string>() { "Average course score", "Average tutor score" };

        List<ReportTableDto> tables = _reportService.GetAverageCoursePointsReport();
        _emailService.SendEmailWithPDFAttachment(recepient, emailSubject, emailBody, pdfName, _pdfReportService.GetReportPDF(reportTitles,tables));
    }

}
