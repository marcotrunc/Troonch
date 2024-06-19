using Troonch.Application.Base.Interfaces;
using System.Net;
using System.Net.Mail;

namespace Troonch.EmailSender.MailTrap.Sender;

public class SMTPMailTrapSender : IEmailSender
{
    public async Task SendEmailAsync(string to, string subject, string body, string from = "")
    {
        var client = new SmtpClient("live.smtp.mailtrap.io", 587)
        {
            Credentials = new NetworkCredential("api", "16e66904d1ba41089c6cba65c3d0d212"),
            EnableSsl = true
        };
        
        to = "marco.truncellito@outlook.it";
        client.Send("mailtrap@demomailtrap.com", to, subject, body);
    }
}