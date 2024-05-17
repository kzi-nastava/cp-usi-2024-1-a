using System.IO;
using System.Net;
using System.Net.Mail;
using PdfSharpCore.Pdf;

namespace LangLang.Application.Utility.Email;

public class EmailService : IEmailService
{
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


        Attachment attachment = new Attachment(pdfStream, "LangLangReport.pdf", "application/pdf");


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

