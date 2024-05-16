using LangLang.Application.UseCases.Email;
using LangLang.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace LangLang.Application.UseCases.Report;

public class ReportCoordinator: IReportCoordinator
{
    private readonly IEmailService _emailService;
    private readonly IReportService _reportService;
    public ReportCoordinator(IEmailService emailService, IReportService reportService) 
    { 
        _emailService = emailService;
        _reportService = reportService;
    }

    public void SendCoursePenaltyReport()
    {
        //SendEmail("masamasa12332@gmail.com");
        PDFReportService pdfReportService = new PDFReportService();

        ReportTableData tableData = _reportService.GetCoursePenaltyReport();

        string recipient = "masamasa12332@gmail.com";
        string title = "Report of penalty statistics";
        string paragraph = "\tThis report details the number of penalty points awarded per course. " +
            "The second table containts the average grade of student based on the number of penalty points they have." +
            "\n\n The data is gathered over the course of 1 year.";

        _emailService.SendEmailWithPDFAttachment(recipient, pdfReportService.GetReportPDF(title, paragraph, tableData.ColumnNames, tableData.Rows));
    }
}
