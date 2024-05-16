using LangLang.Application.UseCases.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Application.UseCases.Report;

public class ReportCoordinator
{
    private readonly IEmailService _emailService;
    public ReportCoordinator(IEmailService emailService) 
    { 
        _emailService = emailService;
    }

    public void SendCoursePenaltyReport()
    {
        //SendEmail("masamasa12332@gmail.com");
        PDFReportService pdfReportService = new PDFReportService();
        List<string> columnNames = new List<string>()
        {
            "name",
            "language",
            "level"
        };
        List<List<string>> list2 = new List<List<string>>();
        for(int i = 0; i < 20; i++)
        {
            list2.Add(new List<string>()
            {
                "marija",
                "parezanin",
                "hey"
            });
        }

        string recipient = "masamasa12332@gmail.com";
        string title = "Report of penalty statistics";
        string paragraph = "\tThis report details the number of penalty points awarded per course. " +
            "The second table containts the average grade of student based on the number of penalty points they have." +
            "\n\n The data is gathered over the course of 1 year.";

        _emailService.SendEmailWithPDFAttachment(recipient, pdfReportService.GetReportPDF(title, paragraph, columnNames, list2));
    }
}
