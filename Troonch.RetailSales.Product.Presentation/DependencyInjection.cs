using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Troonch.RetailSales.Product.Application;
namespace Troonch.RetailSales.Product.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddRetailSalesProductPresentation(this IServiceCollection services, IConfigurationRoot configuration)
    {
        services.AddRetailSalesProductApplication(configuration);
        return services;
    }

}
