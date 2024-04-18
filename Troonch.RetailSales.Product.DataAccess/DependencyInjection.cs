using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Troonch.Sales.DataAccess.DBMSConfiguration;

namespace Troonch.Sales.DataAccess
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfigurationRoot configuration)
        {

            //services.Configure<DatabaseConfiguration>(options => configuration.Bind("DataBaseConfiguration", options));


            //string? connectionString = configuration.GetConnectionString("ConnectionString");
            //string? connectionString = null;
            

            return services;
        }
    }
}
