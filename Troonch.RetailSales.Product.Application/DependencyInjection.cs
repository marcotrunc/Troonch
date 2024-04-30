using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using Troonch.Application.Base.UnitOfWork;
using Troonch.RetailSales.Product.Application.Services;
using Troonch.RetailSales.Product.Application.Validators;
using Troonch.RetailSales.Product.Application.Validators.Resources;
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
        services.AddScoped<ProductGenderService>();
        services.AddScoped<ProductMaterialService>();

        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture("en-GB");

            var cultures = new CultureInfo[]
            {
                new CultureInfo("en-GB"),
                new CultureInfo("it-IT"),
                new CultureInfo("fr"),
                new CultureInfo("de"),
                new CultureInfo("es"),
            };

            options.SupportedCultures = cultures;
            options.SupportedUICultures = cultures;
        });

        services.AddScoped<ResourcesHelper>();

        return services;
    }

}












