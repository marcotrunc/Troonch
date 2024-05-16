using Microsoft.Extensions.Logging;
using Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces;
using Troonch.Sales.Domain.Entities;

namespace Troonch.RetailSales.Product.Application.Services;

public class ProductSizeOptionService
{
    private readonly ILogger<ProductSizeOptionService> _logger;
    private readonly IProductSizeOptionRepository _productSizeOptionRepository;

    public ProductSizeOptionService(
        ILogger<ProductSizeOptionService> logger,
        IProductSizeOptionRepository productSizeOptionRepository
        )
    {
        _logger = logger;
        _productSizeOptionRepository = productSizeOptionRepository; 
    }

    public async Task<IEnumerable<ProductSizeOption>> GetProductSizeOptionsByTypeIdAsync(Guid typeId)
    {
        var productSizeOptions = await _productSizeOptionRepository.GetProductSizeOptionsByTypeIdAsync(typeId);

        if( productSizeOptions is null ) 
        {
            _logger.LogError("ProductSizeOptionService::GetProductSizeOptionsByTypeIdAsync productSizeOptions is null");
            throw new ArgumentNullException(nameof(productSizeOptions));
        }

        return productSizeOptions;
    }
}
