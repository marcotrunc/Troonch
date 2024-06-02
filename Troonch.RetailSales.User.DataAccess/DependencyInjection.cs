using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Troonch.User.DataAccess.DataContext;

namespace Troonch.User.DataAccess;

public static class DependencyInjection
{
    public static IServiceCollection AddUserDataAccess(this IServiceCollection services, IConfigurationRoot configuration)
    {

        string? connectionString = configuration.GetValue<string>("ConnectionStrings:AppDbConnectionString");

        if (!String.IsNullOrWhiteSpace(connectionString))
        {
            services.AddDbContext<UserDataContext>(options =>
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });
        }
        return services;
    }
}
