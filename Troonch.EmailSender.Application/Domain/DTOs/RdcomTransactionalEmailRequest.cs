using Newtonsoft.Json;

namespace Troonch.EmailSender.Rdcom.Domain.DTOs;

public class RdcomTransactionalEmailRequest
{
    [JsonProperty("default",NullValueHandling = NullValueHandling.Ignore)]
    public EmailDefaultBody DefaultBody { get; set; } 
    [JsonProperty("specific")]
    public List<EmailSpecificBody> EmailSpecificBody { get; set; } = new List<EmailSpecificBody>();
}
