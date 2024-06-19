namespace Troonch.EmailSender.Rdcom.Domain.Configuration;

public class RdcomConfiguration
{
    public string Sender { get; set; } = string.Empty;
    public string Username { get; set; }
    public string Password { get; set; }
    public string AccountNumber { get; set; }
}
