using Newtonsoft.Json;

namespace Troonch.EmailSender.Rdcom.Domain.DTOs;

public class RdcomEmailError
{
    [JsonProperty("field")]
    public string Field {  get; set; }

    [JsonProperty("code")]
    public string Code { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }

    [JsonProperty("data")]
    public string Data { get; set; }
}