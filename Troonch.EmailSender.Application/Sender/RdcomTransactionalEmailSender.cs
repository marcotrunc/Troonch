using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Troonch.Application.Base.Interfaces;
using Troonch.EmailSender.Rdcom.Services;

namespace Troonch.EmailSender.Rdcom.Sender;

public class RdcomTransactionalEmailSender : IEmailSender
{
    private readonly ILogger<RdcomTransactionalEmailSender> _logger;
    private readonly RdcomTransactionalEmailService _rdcomTransactionalEmailService;
    private readonly RdComApiAuthenticationService _rdcomApiAuthenticationService;
    public RdcomTransactionalEmailSender(
                                ILogger<RdcomTransactionalEmailSender> logger,
                                RdcomTransactionalEmailService rdcomTransactionalEmailService,
                                RdComApiAuthenticationService rdcomApiAuthenticationService
    )
    {
        _logger = logger;
        _rdcomTransactionalEmailService = rdcomTransactionalEmailService;
        _rdcomApiAuthenticationService = rdcomApiAuthenticationService;
    }
    public async Task SendEmailAsync(string to, string subject, string body, string from = "")
    {
        try
        {
            var accessToken = await _rdcomApiAuthenticationService.GetAccessTokenAsync();

            if (accessToken is null)
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            var emailRequest = _rdcomTransactionalEmailService.BuildEmailsRequest(to, subject, body, from);

            if(emailRequest is null)
            {
                throw new ArgumentNullException(nameof(emailRequest));
            }

            string jsonBody = JsonConvert.SerializeObject(emailRequest);

            var response = await _rdcomTransactionalEmailService.SendEmailAsync(jsonBody, accessToken);

            // SAVE EMAIl IN DB??
        }
        catch (Exception ex) 
        {
            _logger.LogError($"RdcomTransactionalEmailSender::SendEmailAsync -> Email not sent to {to}");
        }
    }
}
