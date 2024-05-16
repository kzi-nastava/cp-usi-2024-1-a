using System;
using System.Net.Mail;
using System.Net;
using PdfSharpCore.Pdf;
using System.IO;
using LangLang.Application.UseCases.Report.CoursePensaltyReport;


namespace LangLang.Application.UseCases.Email;

public class EmailService: IEmailService
{

    public EmailService() {
        SendEmail("masamasa12332@gmail.com");
        PDFReport pdfReportService = new PDFReport();
        SendEmailWithPDFAttachment("masamasa12332@gmail.com", pdfReportService.GetPenaltyPointReportPDF(null!));
    }



    public void SendEmail(string recipient)
    {
        SmtpClient smtpClient = GetServerClient();
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress("language.school.usi@gmail.com", "Lang Lang School");
        mail.To.Add(new MailAddress(recipient));


        mail.Subject = "Checking if emailing works";
        mail.Body = "Wow. It does.";
        smtpClient.Send(mail);
    }

    public void SendEmailWithPDFAttachment(string recipient, PdfDocument document)
    {
        MemoryStream pdfStream = new MemoryStream();
        document.Save(pdfStream, false);
        pdfStream.Position = 0;

        SmtpClient smtpClient = GetServerClient();
        
        MailMessage mail = new MailMessage();

        mail.From = new MailAddress("language.school.usi@gmail.com", "Lang Lang School");
        mail.To.Add(new MailAddress(recipient));


        Attachment attachment = new Attachment(pdfStream, "example.pdf", "application/pdf");


        mail.Subject = "Checking if emailing works";
        mail.Body = "Wow. It does.";
        mail.Attachments.Add(attachment);

        smtpClient.Send(mail);

        attachment.Dispose();
        pdfStream.Dispose();
        document.Dispose();
    }


    private SmtpClient GetServerClient()
    {
        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
        smtpClient.Credentials = new NetworkCredential("language.school.usi@gmail.com", "vrgtvsjgnbnazxjj");
        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtpClient.EnableSsl = true;
        return smtpClient;
    }
}

