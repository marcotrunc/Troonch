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

        services.Configure<IdentityOptions>(options =>
        {
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.MaxValue;

            options.SignIn.RequireConfirmedEmail = true;
        });

        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(1);

            options.LoginPath = "/Auth/Login";
            options.AccessDeniedPath = "/Auth/AccessDenied";
            options.SlidingExpiration = true;
        });


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
