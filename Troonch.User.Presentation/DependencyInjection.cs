using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Troonch.User.Application;
using Troonch.Users.Controllers;

namespace Troonch.User.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddUserPresentation(this IServiceCollection services, IConfigurationRoot configuration)
    {
       
        // The build action of cshtml properties in class library must become embeded resource
        var assembly = typeof(UsersController).Assembly;

        services.AddControllersWithViews()
                    .AddApplicationPart(typeof(UsersController).Assembly)
                    .AddRazorRuntimeCompilation(options =>
                    {
                        options.FileProviders.Add(new EmbeddedFileProvider(assembly));
                    });
           
        services.AddUserService(configuration);

        return services;
    }
}
