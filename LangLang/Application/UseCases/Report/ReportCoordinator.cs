using LangLang.Application.DTO;
using LangLang.Application.UseCases.Email;


namespace LangLang.Application.UseCases.Report;

public class ReportCoordinator: IReportCoordinator
{
    private readonly IEmailService _emailService;
    private readonly IReportService _reportService;
    public ReportCoordinator(IEmailService emailService, IReportService reportService) 
    { 
        _emailService = emailService;
        _reportService = reportService;
        SendCoursePenaltyReport();

    }

    public void SendCoursePenaltyReport()
    {
        //SendEmail("masamasa12332@gmail.com");
        PDFReportService pdfReportService = new PDFReportService();

        ReportTableDto tableData = _reportService.GetCoursePenaltyReport();

        string recipient = "masamasa12332@gmail.com";
        string title = "Report of penalty statistics";
        string paragraph = "This report details the number of penalty points awarded per course. " +
            "The second table containts the average grade of student based on the number of penalty points they have." +
            "\nThe data is gathered over the course of 1 year.";

        _emailService.SendEmailWithPDFAttachment(recipient, pdfReportService.GetReportPDF(title, paragraph, tableData));
    }
}
