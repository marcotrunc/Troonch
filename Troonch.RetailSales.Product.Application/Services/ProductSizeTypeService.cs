using Microsoft.Extensions.Logging;
using Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces;
using Troonch.Sales.Domain.Entities;

namespace Troonch.RetailSales.Product.Application.Services;

public class ProductSizeTypeService
{
    private readonly ILogger<ProductSizeTypeService> _logger;
    private readonly IProductSizeTypeRepository _productSizeTypeRepository;
    public ProductSizeTypeService(
                ILogger<ProductSizeTypeService> logger, 
                IProductSizeTypeRepository productSizeTypeRepository
    )
    {
        _logger = logger;
        _productSizeTypeRepository = productSizeTypeRepository;
    }

    public async Task<IEnumerable<ProductSizeType>> GetAllProductSizeTypesAsync()
    {
        var productSizeTypes = await _productSizeTypeRepository.GetAllAsync();

        if (productSizeTypes is null) 
        {
            _logger.LogError("ProductSizeTypeService::GetAllProductSizeTypesAsync productSizeTypes is null");
            throw new ArgumentNullException(nameof(productSizeTypes));
        }

        return productSizeTypes;
    }
}
