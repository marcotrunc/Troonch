using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Reflection;
using Troonch.Application.Base.Utilities;
using Troonch.EmailSender.Rdcom.Domain;
using Troonch.EmailSender.Rdcom.Domain.Configuration;
using Troonch.EmailSender.Rdcom.Domain.DTOs;

namespace Troonch.EmailSender.Rdcom.Services;

public class RdcomTransactionalEmailService
{
    private readonly RdcomEndPointConfiguration _endpointConfiguration;
    private readonly RdcomConfiguration _rdcomConfiguration;
    private readonly ILogger<RdcomTransactionalEmailService> _logger;
    private readonly string resourceName = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\RdcomEndpoints.json";
    public RdcomTransactionalEmailService(
                ILogger<RdcomTransactionalEmailService> logger,
                IOptions<RdcomConfiguration> rdcomConfiguration

    )
    {
        _logger = logger;
        _endpointConfiguration = JsonLoaderUtility<RdcomEndPointConfiguration>.LoadFromEmbeddedResource(resourceName, _logger);
        _rdcomConfiguration = rdcomConfiguration.Value;
    }

    public async Task<IEnumerable<RdcomTransactionalEmailResponse>?> SendEmailAsync(string jsonBody, string accessToken)
    {
        if (String.IsNullOrEmpty(jsonBody))
        {
            throw new ArgumentNullException(nameof(jsonBody));
        }
        if (String.IsNullOrEmpty(accessToken)) 
        {
            throw new ArgumentNullException(nameof(accessToken));
        }

        string apiUrl = $"{_endpointConfiguration.BaseUri}{_endpointConfiguration.TransactionalEmail}".Replace("{account_code}", _rdcomConfiguration.AccountNumber);

        IEnumerable<RdcomTransactionalEmailResponse> rdcomTransactionalEmailResponse = null;

        var httpClient = new HttpClientUtility(accessToken);

        var response = await httpClient.DoRequest(apiUrl, jsonBody, HttpClientUtility.WebMethods.Post);

        if(response is null)
        {
            _logger.LogError($"RdcomTransactionalEmailService::SendEmailAsync -> response is null, email not sent, this is request: {jsonBody}");
            throw new ArgumentNullException(nameof(response));
        }

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError($"RdcomTransactionalEmailService::SendEmailAsync -> response status code is {response.StatusCode}, email not sent, this is request: {jsonBody}");
            throw new Exception(nameof(response));
        }

        string jsonContent = await response.Content.ReadAsStringAsync();

        rdcomTransactionalEmailResponse = JsonConvert.DeserializeObject<List<RdcomTransactionalEmailResponse>>(jsonContent) ??  throw new Exception(nameof(rdcomTransactionalEmailResponse));

        return rdcomTransactionalEmailResponse;

    }

    public RdcomTransactionalEmailRequest BuildEmailsRequest(string to, string subject, string body, string from = "") =>
                       new RdcomTransactionalEmailRequest()
                       {
                           DefaultBody = new EmailDefaultBody
                           {
                               ContentHtml = body,
                               Subject = subject,
                               SenderAddress = !String.IsNullOrWhiteSpace(from) ? from : null,
                               Track = true
                           },
                           EmailSpecificBody = new List<EmailSpecificBody>
                               {
                                    new EmailSpecificBody{Address = to}
                               }
                       };
}
