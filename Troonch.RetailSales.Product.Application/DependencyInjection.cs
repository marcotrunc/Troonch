using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Troonch.Application.Base.UnitOfWork;
using Troonch.RetailSales.Product.Application.Services;
using Troonch.RetailSales.Product.Application.Validators;
using Troonch.Sales.DataAccess;

namespace Troonch.RetailSales.Product.Application;

    public static class DependencyInjection
    {
    public static IServiceCollection AddRetailSalesProductApplication(this IServiceCollection services, IConfigurationRoot configuration)
    {
        services.AddDataAccess(configuration);
        services.AddValidatorsFromAssemblyContaining<ProductBrandReqValidator>();
        services.AddScoped<IUnitOfWork, UnitOfWork<RetailSalesProductDataContext>>();
        services.AddScoped<ProductBrandService>();
        services.AddScoped<ProductCategoryServices>();
        services.AddScoped<ProductServices>();

        return services;
    }

}












