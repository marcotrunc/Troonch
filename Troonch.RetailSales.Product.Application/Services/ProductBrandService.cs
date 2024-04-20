using FluentValidation;
using Microsoft.Extensions.Logging;
using Slugify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troonch.Application.Base.UnitOfWork;
using Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces;
using Troonch.RetailSales.Product.Domain.DTOs.Requests;
using Troonch.Sales.Domain.Entities;

namespace Troonch.RetailSales.Product.Application.Services;

public class ProductBrandService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductBrandRepository _productBrandRepository;
    private readonly IValidator<ProductBrandRequestDTO> _validator;
    private readonly ILogger<ProductBrandService> _logger;
    public ProductBrandService(IUnitOfWork unitOfWork, IProductBrandRepository  productBrandRepository, IValidator<ProductBrandRequestDTO> validator, ILogger<ProductBrandService> logger)
    {
        _unitOfWork = unitOfWork;
        _productBrandRepository = productBrandRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<IEnumerable<ProductBrand>> GetAllProductBrandAsync()
    {
        var productBrands = await _productBrandRepository.GetAllAsync();

        if(productBrands is null) 
        {
            _logger.LogError("ProductBrandService::GetAllProductBrandAsync productBrands is null");
            throw new ArgumentNullException(nameof(productBrands));
        }

        return productBrands;
    }

    public async Task<ProductBrand> GetProductBrandByAsync(Guid id)
    {
        if(id == Guid.Empty)
        {
            _logger.LogError("ProductBrandService::GetProductBrandByAsync id is Guid.Empty");
            throw new ArgumentNullException(nameof(id));
        }

        var productBrand = await _productBrandRepository.GetByIdAsync(id);

        if(productBrand is null)
        {
            _logger.LogError("ProductBrandService::GetProductBrandByAsync productBrand is null");
            throw new ArgumentNullException(nameof(productBrand));
        }
        
        return productBrand;
    }

    public async Task<bool> AddProductBrandAsync(ProductBrandRequestDTO productBrandRequest)
    {
        if(productBrandRequest is null) 
        {
            _logger.LogError("ProductBrandService::AddProductBrandAsync productBrandRequest is null");
            throw new ArgumentNullException(nameof(productBrandRequest));
        }

        await _validator.ValidateAndThrowAsync(productBrandRequest);

        var slugHelper = new SlugHelper();

        var brandToAdd = new ProductBrand
        {
            Name = productBrandRequest.Name,
            Description = productBrandRequest.Description,
            Slug = slugHelper.GenerateSlug(productBrandRequest.Name)
        };

        var productAdded = await _productBrandRepository.AddAsync(brandToAdd);

        if (productAdded is null)
        {
            _logger.LogError("ProductBrandService::AddProductBrandAsync productAdded is null");
            throw new ArgumentException(nameof(productAdded));
        }

        var isProductBrandSaved = await _unitOfWork.CommitAsync();

        if (!isProductBrandSaved)
        {
            _logger.LogError("ProductBrandService::AddProductBrandAsync isProductBrandSaved is false");
            throw new Exception(nameof(isProductBrandSaved));
        }

        return true;
    }


}
