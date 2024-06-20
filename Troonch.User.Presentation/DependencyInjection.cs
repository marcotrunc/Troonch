using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Troonch.User.Application;
using Troonch.User.DataAccess.DataContext;
using Troonch.User.Domain.Entities;
using Troonch.Users.Controllers;

namespace Troonch.User.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddUserPresentation(this IServiceCollection services, IConfigurationRoot configuration)
    {
        services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                                                                .AddRoles<IdentityRole>()
                                                                .AddEntityFrameworkStores<UserDataContext>();

        
        
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
