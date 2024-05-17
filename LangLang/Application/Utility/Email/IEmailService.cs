namespace LangLang.Application.Utility.Email;
using PdfSharpCore.Pdf;

public interface IEmailService
{
    public void SendEmail(string recipient);
    public void SendEmailWithPDFAttachment(string recipient, PdfDocument document);
}
