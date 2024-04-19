using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Troonch.Application.Base.UnitOfWork;
using Troonch.Sales.DataAccess;

namespace Troonch.RetailSales.Product.Application;

    public static class DependencyInjection
    {
    public static IServiceCollection AddRetailSalesProductApplication(this IServiceCollection services, IConfigurationRoot configuration)
    {
        services.AddDataAccess(configuration);
        services.AddScoped<IUnitOfWork, UnitOfWork<RetailSalesProductDataContext>>();

        return services;
    }

}












