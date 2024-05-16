using Microsoft.Extensions.Logging;
using Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces;
using Troonch.Sales.Domain.Entities;

namespace Troonch.RetailSales.Product.Application.Services;

public class ProductColorService
{
    private readonly ILogger<ProductColorService> _logger;
    private readonly IProductColorRepository _productColorRepository;

    public ProductColorService(
        ILogger<ProductColorService> logger,
        IProductColorRepository productColorRepository
        )
    {
        _logger = logger;
        _productColorRepository = productColorRepository;  
    }

    public async Task<IEnumerable<ProductColor>> GetProductColorsAsync()
    {
        var productColors = await _productColorRepository.GetAllAsync();

        if( productColors is null )
        {
            _logger.LogError("ProductColorService::GetProductColorsAsync productColors is null");
            throw new ArgumentNullException(nameof(productColors));
        }

        return productColors;
    }
}
