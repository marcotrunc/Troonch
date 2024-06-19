using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Troonch.Application.Base.Interfaces;
using Troonch.EmailSender.MailTrap.Sender;
namespace Troonch.EmailSender.MailTrap;

public static class DependencyInjection
{
    public static IServiceCollection AddEmailSenderMailTrap(this IServiceCollection services, IConfigurationRoot configuration)
    {
        
        services.AddScoped<IEmailSender, SMTPMailTrapSender>();

        return services;
    }
}
