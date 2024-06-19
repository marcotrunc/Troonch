using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Troonch.Application.Base.Interfaces;
using Troonch.EmailSender.Rdcom.Sender;
using Troonch.EmailSender.Rdcom.Services;

namespace Troonch.EmailSender.Rdcom;

public static class DependencyInjection
{
    public static IServiceCollection AddEmailSenderRdcom(this IServiceCollection services, IConfigurationRoot configuration)
    {
        services.AddTransient<RdcomTransactionalEmailService>();
        services.AddTransient<RdComApiAuthenticationService>();
        services.AddTransient<IEmailSender,RdcomTransactionalEmailSender>();

        return services;
    }
}
