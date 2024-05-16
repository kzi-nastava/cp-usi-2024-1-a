using System;
using System.Net.Mail;
using System.Net;


namespace LangLang.Application.UseCases.Email;

public class EmailService: IEmailService
{

    public EmailService() {
        SendEmail("masamasa12332@gmail.com");
    }



    public void SendEmail(string recipient)
    {
        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

        smtpClient.Credentials = new NetworkCredential("language.school.usi@gmail.com", "vrgtvsjgnbnazxjj");
        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtpClient.EnableSsl = true;
        MailMessage mail = new MailMessage();

        //Setting From , To and CC
        mail.From = new MailAddress("language.school.usi@gmail.com", "Lang Lang School");
        mail.To.Add(new MailAddress(recipient));
        mail.CC.Add(new MailAddress(recipient));


        mail.Subject = "Checking if emailing works";
        mail.Body = "Wow. It does.";
        smtpClient.Send(mail);
    }
}

