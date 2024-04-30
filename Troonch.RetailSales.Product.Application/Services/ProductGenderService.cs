using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces;
using Troonch.Sales.Domain.Entities;

namespace Troonch.RetailSales.Product.Application.Services;

public class ProductGenderService
{
    private readonly ILogger<ProductGenderService> _logger;
    private readonly IProductGenderRepository _productGenderRepository;
    public ProductGenderService(ILogger<ProductGenderService> logger, IProductGenderRepository productGenderRepository)
    {
        _logger = logger;
        _productGenderRepository = productGenderRepository;
    }

    public async Task<IEnumerable<ProductGender>> GetProductGendersAsync()
    {
        var productGenders = await _productGenderRepository.GetAllAsync();

        if (productGenders is null)
        {
            _logger.LogError("ProductGenderService::GetProductGendersAsync productGenders is null");
            throw new ArgumentNullException(nameof(productGenders));
        }

        return productGenders;
    }
}
