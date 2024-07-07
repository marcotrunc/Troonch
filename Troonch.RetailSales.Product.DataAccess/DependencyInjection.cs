using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Troonch.RetailSales.Product.DataAccess.Repositories;
using Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces;

namespace Troonch.Sales.DataAccess
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddScoped<IProductBrandRepository,ProductBrandRepository>();
            services.AddScoped<IProductCategoryRepository,ProductCategoryRepository>(); 
            services.AddScoped<IProductSizeTypeRepository,ProductSizeTypeRepository>();
            services.AddScoped<IProductGenderRepository,ProductGenderRepository>(); 
            services.AddScoped<IProductMaterialRepository,ProductMaterialRepository>(); 
            services.AddScoped<IProductSizeOptionRepository,ProductSizeOptionRepository>();
            services.AddScoped<IProductItemRepository,ProductItemRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            string? connectionString = configuration.GetValue<string>("ConnectionStrings:AppDbConnectionString");
            if (!String.IsNullOrWhiteSpace(connectionString))
            {
                services.AddDbContext<RetailSalesProductDataContext>(options =>
                {
                    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                });
            }

            return services;
        }
    }
}
