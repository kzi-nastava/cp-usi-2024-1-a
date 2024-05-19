namespace LangLang.Application.Utility.Email;
using PdfSharpCore.Pdf;

public interface IEmailService
{
    public void SendEmail(string recipient, string emailSubject, string emailBody);
    public void SendEmailWithPDFAttachment(string recipient, string emailSubject, string emailBody, string pdfTitle, PdfDocument document);
}
