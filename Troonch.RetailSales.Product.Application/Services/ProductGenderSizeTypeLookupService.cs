using Microsoft.Extensions.Logging;
using Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces;
using Troonch.Sales.Domain.Entities;

namespace Troonch.RetailSales.Product.Application.Services;

public class ProductGenderSizeTypeLookupService
{
    private readonly ILogger<ProductMaterialService> _logger;
    private readonly IProductGenderSizeTypeLookupRepository _productGenderSizeTypeLookupRepository;
    public ProductGenderSizeTypeLookupService(
        ILogger<ProductMaterialService> logger,
        IProductGenderSizeTypeLookupRepository productGenderSizeTypeLookupRepository
        )
    {                          
        _logger = logger;
        _productGenderSizeTypeLookupRepository = productGenderSizeTypeLookupRepository; 
    }

    public async Task<IEnumerable<ProductGenderSizeTypeLookup>> GetAllProductGenderSizeTypesAsync()
    {
        var productgenderSizeTypes = await _productGenderSizeTypeLookupRepository.GetAllAsync();

        if(productgenderSizeTypes is null ) 
        {
            _logger.LogError("ProductGenderSizeTypeLookupService::GetAllProductGenderSizeTypesAsync productgenderSizeTypes is null");
            throw new ArgumentNullException(nameof(productgenderSizeTypes));
        }

        return productgenderSizeTypes;
    }
}
