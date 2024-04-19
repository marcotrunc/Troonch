using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Troonch.Sales.DataAccess;
using Troonch.RetailSales.Product.Application;

namespace Troonch.CA.Test
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            IServiceProvider services = null;
            try
            {
                var host = Host.CreateDefaultBuilder(args)
                                    .ConfigureServices(ConfigureServices)
                                    //.UseSerilog((context, configuration) =>
                                    //        configuration.ReadFrom.Configuration(context.Configuration))
                                    .Build();

                services = host.Services;

                await services.GetRequiredService<TestApp>().Run(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void ConfigureServices(HostBuilderContext hostingContext, IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString("AppDbConnectionString");
            services.AddDbContext<RetailSalesProductDataContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            services.AddRetailSalesProductApplication(configuration);

            services.AddSingleton<TestApp>();
        }
    }
}
