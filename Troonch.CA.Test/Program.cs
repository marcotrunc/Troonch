using Serilog;
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
                                    .UseSerilog((context, configuration) =>
                                            configuration.ReadFrom.Configuration(context.Configuration))
                                    .Build();
                
                services = host.Services;

                await services.GetRequiredService<TestApp>().Run(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }

        private static void ConfigureServices(HostBuilderContext hostingContext, IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .Build();

            services.AddRetailSalesProductApplication(configuration);

            services.AddScoped<TestApp>();
        }
    }
}
