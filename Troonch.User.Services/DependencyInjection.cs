using FluentValidation;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Troonch.User.Application.Services;
using Troonch.User.Application.Validators;
using Troonch.User.DataAccess;

namespace Troonch.User.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddUserService(this IServiceCollection services, IConfigurationRoot configuration)
    {
        services.AddUserDataAccess(configuration);
        services.AddValidatorsFromAssemblyContaining<LoginReqValidator>();
        services.AddScoped<UserService>();
        services.AddScoped<AuthService>();
        services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

        return services;
    }
}
