using System.IO;
using System.Net;
using System.Net.Mail;
using PdfSharpCore.Pdf;

namespace LangLang.Application.Utility.Email;

public class EmailService : IEmailService
{
    private readonly EmailCredentials _emailCredentials;

    public EmailService(EmailCredentials emailCredentials)
    {
        _emailCredentials = emailCredentials;
    }

    public void SendEmail(string recipient, string emailSubject, string emailBody)
    {
        SmtpClient smtpClient = GetServerClient();
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress(_emailCredentials.EmailAddress, "Lang Lang School");
        mail.To.Add(new MailAddress(recipient));


        mail.Subject = emailSubject;
        mail.Body = emailBody;
        smtpClient.Send(mail);
    }

    public void SendEmailWithPDFAttachment(string recipient, string emailSubject, string emailBody, string pdfTitle, PdfDocument document)
    {
        MemoryStream pdfStream = new MemoryStream();
        document.Save(pdfStream, false);
        pdfStream.Position = 0;

        SmtpClient smtpClient = GetServerClient();

        MailMessage mail = new MailMessage();

        mail.From = new MailAddress(_emailCredentials.EmailAddress, "Lang Lang School");
        mail.To.Add(new MailAddress(recipient));


        Attachment attachment = new Attachment(pdfStream, pdfTitle, "application/pdf");


        mail.Subject = emailSubject;
        mail.Body = emailBody;
        mail.Attachments.Add(attachment);

        smtpClient.Send(mail);

        attachment.Dispose();
        pdfStream.Dispose();
        document.Dispose();
    }


    private SmtpClient GetServerClient()
    {
        var smtpClient = new SmtpClient(_emailCredentials.Host, _emailCredentials.Port);
        smtpClient.Credentials = new NetworkCredential(_emailCredentials.EmailAddress, _emailCredentials.Password);
        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtpClient.EnableSsl = true;
        return smtpClient;
    }
}