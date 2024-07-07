using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Troonch.RetailSales.Product.Application;
using Troonch.RetailSales.Product.Presentation.Controllers;
namespace Troonch.RetailSales.Product.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddRetailSalesProductPresentation(this IServiceCollection services, IConfigurationRoot configuration)
    {
        services.AddRetailSalesProductApplication(configuration);

        var assembly = typeof(CategoriesController).Assembly;

        services.AddControllersWithViews()
                    .AddApplicationPart(typeof(CategoriesController).Assembly)
                    .AddRazorRuntimeCompilation(options =>
                    {
                        options.FileProviders.Add(new EmbeddedFileProvider(assembly));
                    });

        return services;
    }

}
