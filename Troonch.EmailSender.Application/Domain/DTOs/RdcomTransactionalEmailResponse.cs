using Newtonsoft.Json;

namespace Troonch.EmailSender.Rdcom.Domain.DTOs;

public class RdcomTransactionalEmailResponse
{
    /// <summary>
    /// Unique identifier generated for the email
    /// </summary>
    [JsonProperty("uuid")]
    public string Id { get; set; }

    /// <summary>
    /// This value represents the index (in the 'specific' array) of object passed by the APIs body, starting from 0
    /// </summary>
    [JsonProperty("index")]
    public int Index { get; set; }

    /// <summary>
    /// An array of errors for the specific email. If empty means the email has been correctly submitted
    /// </summary>
    [JsonProperty("errors")]
    public IEnumerable<RdcomEmailError>? Errors { get; set; } = null;
}
