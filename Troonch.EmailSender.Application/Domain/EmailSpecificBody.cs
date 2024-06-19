using Newtonsoft.Json;

namespace Troonch.EmailSender.Rdcom.Domain;

public class EmailSpecificBody
{
    /// <summary>
    /// The email recipient. This is the only information that can be provided only in the objects of the 'specific' array,
    /// not in the 'deafult' object
    /// </summary>
    [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// The sender that you want to send emails from
    /// </summary>
    [JsonProperty("sender_address", NullValueHandling = NullValueHandling.Ignore)]
    public string SenderAddress { get; set; }

    /// <summary>
    /// The name of the sender you want to send emails from
    /// </summary>
    [JsonProperty("sender_name", NullValueHandling = NullValueHandling.Ignore)]
    public string SenderName {  get; set; }

    /// <summary>
    /// The address where you want to eventually receive replies
    /// </summary>
    [JsonProperty("reply_to_address", NullValueHandling = NullValueHandling.Ignore)]
    public string? ReplyToAddress { get; set; }

    /// <summary>
    /// The name that appears as the sender to reply to
    /// </summary>
    [JsonProperty("reply_to_name", NullValueHandling = NullValueHandling.Ignore)]
    public string ReplyToName { get; set; }

    /// <summary>
    /// The name that will be shown in the 'To:'' field of the email
    /// </summary>
    [JsonProperty("to_name", NullValueHandling = NullValueHandling.Ignore)]
    public string ToName { get; set; }

    /// <summary>
    /// Subject of the email
    /// </summary>
    [JsonProperty("subject", NullValueHandling = NullValueHandling.Ignore)]
    public string Subject { get; set; }

    /// <summary>
    /// Textual preview of the email
    /// </summary>
    [JsonProperty("preview_text", NullValueHandling = NullValueHandling.Ignore)]
    public string Previewtext {  get; set; }

    /// <summary>
    /// The content of the email in HTML format. 
    /// At least one between 'template_code' and 'content_html' must be provided
    /// </summary>
    [JsonProperty("content_html", NullValueHandling = NullValueHandling.Ignore)]
    public string ContentHtml { get; set; } 

    /// <summary>
    /// Code of the template that will be used for the email content. 
    /// At least one between 'template_code' and 'content_html' must be provided
    /// </summary>
    [JsonProperty("template_code", NullValueHandling = NullValueHandling.Ignore)]
    public string TemplateCode { get; set; }

    /// <summary>
    /// Json object composed of key value pairs for template variables substitution. This will me merged with the meta object present in the default and will overwrite any existing value. 
    /// If a variable present in the template is not present in this object or in the default one, it will be left untouched.
    /// </summary>
    [JsonProperty("meta", NullValueHandling = NullValueHandling.Ignore)]
    public object Meta { get; set; }

    /// <summary>
    /// Set to true if you want to track the links of the emails of this email. By default is set to false
    /// </summary>
    [JsonProperty("track", NullValueHandling = NullValueHandling.Ignore)]
    public object Track { get; set; }

    /// <summary>
    /// Email sending schedulation
    /// </summary>
    [JsonProperty("schedule_time", NullValueHandling = NullValueHandling.Ignore)]
    public string ScheduleTime { get; set; }

    /// <summary>
    /// The timezone to use for the email sending schedulation
    /// </summary>
    [JsonProperty("timezone", NullValueHandling = NullValueHandling.Ignore)]
    public string TimeZone { get; set; }

    /// <summary>
    /// The name of the gateway you want to use for this sending
    /// </summary>
    [JsonProperty("gateway", NullValueHandling = NullValueHandling.Ignore)]
    public string Gateway { get; set; }

    /// <summary>
    /// Array of objects where each object must contain the following 
    /// keys: 'filename', 'content_type', 'content_base64'. Accepted 'content_type' are jpg, jpe, jpeg, gif, png, pdf, odt, odp, ods, doc, 
    /// docx, xls, xlsx, ppt, pptx, txt, csv, tar, rar and zip.
    /// </summary>
    [JsonProperty("attachments", NullValueHandling = NullValueHandling.Ignore)]
    public IEnumerable<object> Attachments { get; set; }

    /// <summary>
    /// The external reference that you want to put to a specific sending.
    /// </summary>
    [JsonProperty("external_reference", NullValueHandling = NullValueHandling.Ignore)]
    public string ExternalReference { get; set; }
}
