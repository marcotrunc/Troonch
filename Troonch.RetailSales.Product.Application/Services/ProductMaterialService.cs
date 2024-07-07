using Microsoft.Extensions.Logging;
using Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces;
using Troonch.Sales.Domain.Entities;

namespace Troonch.RetailSales.Product.Application.Services;

public class ProductMaterialService
{
    private readonly ILogger<ProductMaterialService> _logger;
    private readonly IProductMaterialRepository _productMaterialRepository;
    public ProductMaterialService(
        ILogger<ProductMaterialService> logger,
        IProductMaterialRepository productMaterialRepository
        )
    {                          
        _logger = logger;
        _productMaterialRepository = productMaterialRepository; 
    }

    public async Task<IEnumerable<ProductMaterial>> GetAllProductMaterialAsync()
    {
        var productMaterials = await _productMaterialRepository.GetAllAsync(null);

        if( productMaterials is null ) 
        {
            _logger.LogError("ProductMaterialService::GetAllProductMaterialAsync productMaterials is null");
            throw new ArgumentNullException(nameof(productMaterials));
        }

        return productMaterials;
    }
}
