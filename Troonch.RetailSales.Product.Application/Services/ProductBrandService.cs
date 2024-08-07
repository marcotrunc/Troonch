﻿using FluentValidation;
using Microsoft.Extensions.Logging;
using Troonch.Application.Base.UnitOfWork;
using Troonch.Application.Base.Utilities;
using Troonch.DataAccess.Base.Helpers;
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

    public async Task<PagedList<ProductBrand>> GetAllProductBrandAsync(string? searchTerm, int page = 1, int pagesize = 10)
    {
        var productBrands = await _productBrandRepository.GetAllAsync(searchTerm);

        if(productBrands is null) 
        {
            _logger.LogError("ProductBrandService::GetAllProductBrandAsync productBrands is null");
            throw new ArgumentNullException(nameof(productBrands));
        }

        return PagedList<ProductBrand>.Create(productBrands, page, pagesize);
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
    public async Task<ProductBrandRequestDTO> GetProductBrandForUpdateByAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            _logger.LogError("ProductBrandService::GetProductBrandByAsync id is Guid.Empty");
            throw new ArgumentNullException(nameof(id));
        }

        var productBrand = await _productBrandRepository.GetByIdAsync(id);

        if (productBrand is null)
        {
            _logger.LogError("ProductBrandService::GetProductBrandByAsync productBrand is null");
            throw new ArgumentNullException(nameof(productBrand));
        }

        return MapFromProductBrandToBroductBrandRequest(productBrand);
    }

    public async Task<bool> AddProductBrandAsync(ProductBrandRequestDTO productBrandRequest)
    {
        if(productBrandRequest is null) 
        {
            _logger.LogError("ProductBrandService::AddProductBrandAsync productBrandRequest is null");
            throw new ArgumentNullException(nameof(productBrandRequest));
        }

        await _validator.ValidateAndThrowAsync(productBrandRequest);


        var brandToAdd = new ProductBrand
        {
            Name = productBrandRequest.Name.Trim(),
            Description = productBrandRequest.Description,
            Slug = SlugUtility.GenerateSlug(productBrandRequest.Name)
        };

        var brandAdded = await _productBrandRepository.AddAsync(brandToAdd);

        if (brandAdded is null)
        {
            _logger.LogError("ProductBrandService::AddProductBrandAsync productAdded is null");
            throw new ArgumentException(nameof(brandAdded));
        }

        var isProductBrandSaved = await _unitOfWork.CommitAsync();

        if (!isProductBrandSaved)
        {
            _logger.LogError("ProductBrandService::AddProductBrandAsync isProductBrandSaved is false");
            return false;
        }

        return true;
    }

    public async Task<bool> UpdateProductBrandAsync(Guid id, ProductBrandRequestDTO productBrandRequest)
    {
        if (id == Guid.Empty) {
            _logger.LogError("ProductBrandService::UpdateProductBrandAsync id is empty");
            throw new ArgumentNullException(nameof(id));
        }
        
        if(productBrandRequest is null)
        {
            _logger.LogError("ProductBrandService::UpdateProductBrandAsync productBrandRequest is empty");
            throw new ArgumentNullException(nameof(productBrandRequest));
        }

        var brandToUpdate = await _productBrandRepository.GetByIdAsync(id);

        if(brandToUpdate is null)
        {
            _logger.LogError("ProductBrandService::UpdateProductBrandAsync brandToUpdate is empty");
            throw new ArgumentNullException(nameof(brandToUpdate));
        }

        productBrandRequest.Id = id;

        await _validator.ValidateAndThrowAsync(productBrandRequest);

        brandToUpdate.Name = productBrandRequest.Name.Trim();
        brandToUpdate.Slug = SlugUtility.GenerateSlug(productBrandRequest.Name);
        brandToUpdate.Description = productBrandRequest.Description;

        var isBrandUpdated = await _unitOfWork.UpdateAsync(brandToUpdate);

        if(!isBrandUpdated)
        {
            _logger.LogError("ProductBrandService::UpdateProductBrandAsync isBrandUpdated is false");
            return false;
        }

        return true;
    }

    public async Task<bool> RemoveProductBrandAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            _logger.LogError("ProductBrandService::RemoveProductBrandAsync id is empty");
            throw new ArgumentNullException(nameof(id));
        }

        var brandToRemove = await _productBrandRepository.GetByIdAsync(id);

        if(brandToRemove is null)
        {
            _logger.LogError("ProductBrandService::RemoveProductBrandAsync brandToRemove not exists");
            throw new ArgumentNullException(nameof(brandToRemove)); 
        }
        
        var isBrandDeletable = await _productBrandRepository.IsDeletableAsync(id);
        

        if (!isBrandDeletable)
        {
            throw new ValidationException($"Too product related to {brandToRemove.Name}, delete product first");
        }

        _productBrandRepository.Delete(brandToRemove);

        return await _unitOfWork.CommitAsync();

    }

    private ProductBrandRequestDTO MapFromProductBrandToBroductBrandRequest(ProductBrand productBrand) => new ProductBrandRequestDTO
    {
        Id = productBrand.Id,
        Name = productBrand.Name,
        Description = productBrand.Description
    };
}
