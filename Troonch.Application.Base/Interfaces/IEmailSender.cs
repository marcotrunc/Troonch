namespace Troonch.Application.Base.Interfaces;

public interface IEmailSender
{
    Task SendEmailAsync(string to, string subject, string body, string from = "");
}
