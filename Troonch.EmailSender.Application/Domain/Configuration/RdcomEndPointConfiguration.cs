using System.ComponentModel.DataAnnotations;

namespace Troonch.EmailSender.Rdcom.Domain.Configuration;

public class RdcomEndPointConfiguration
{
    [Required]
    public string BaseUri { get; set; }
    [Required]
    public string TransactionalEmail { get; set; }
    [Required]
    public string GetToken { get; set; }
}
