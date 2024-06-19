using Newtonsoft.Json;

namespace Troonch.EmailSender.Rdcom.Domain.DTOs;

public class RdcomAuthenticationResponse
{
    /// <summary>
    /// AccessToken.
    /// </summary>
    [JsonProperty("token")]
    public string AccessToken { get; set; }

    /// <summary>
    /// Expire Date
    /// </summary>
    [JsonProperty("expire_date")]
    public DateTime? ExpireDate { get; set; } = null;
}
