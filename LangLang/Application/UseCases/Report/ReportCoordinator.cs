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
        string emailBody = "You've requested a report about the number of penalty points awarded by course. You can find said reported attached bellow.\n\n";
        string pdfName = "LangLang Report";
        string pdfTextTitle = "Report of penalty statistics";

        List<ReportTableDto> tables = _reportService.GetCoursePenaltyReport();
        _emailService.SendEmailWithPDFAttachment(recipient, emailSubject, emailBody, pdfName, _pdfReportService.GetReportPDF(pdfTextTitle, tables));
    }
}
