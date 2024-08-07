﻿using FluentValidation;
using Microsoft.Extensions.Logging;
using Troonch.Application.Base.UnitOfWork;
using Troonch.DataAccess.Base.Helpers;
using Troonch.RetailSales.Product.DataAccess.Repositories;
using Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces;
using Troonch.RetailSales.Product.Domain.DTOs.Requests;
using Troonch.RetailSales.Product.Domain.DTOs.Responses;
using Troonch.Sales.Domain.Entities;

namespace Troonch.RetailSales.Product.Application.Services;

public class ProductCategoryServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly ILogger<ProductCategoryServices> _logger;
    private readonly  IValidator<ProductCategoryRequestDTO> _validator;
    public ProductCategoryServices(IUnitOfWork unitOfWork, IProductCategoryRepository productCategoryRepository, ILogger<ProductCategoryServices> logger, IValidator<ProductCategoryRequestDTO> validator)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _productCategoryRepository = productCategoryRepository;
        _validator = validator;
    }

    public async Task<PagedList<ProductCategoryResponseDTO>> GetProductCategoriesAsync(string? searchTerm, int page = 1, int pagesize = 10)
    {
        var productCategories = await _productCategoryRepository.GetAllProductCategoriesWithSizeAsync(searchTerm); 

        if(productCategories is null)
        {
            _logger.LogError("ProductCategoryServices::GetProductCategoriesAsync productCategories is null");
            throw new ArgumentNullException(nameof(productCategories));
        }


        var productCategoriesMapped = productCategories.Select(pc => new ProductCategoryResponseDTO
        { Id = pc.Id, Name = pc.Name, ProductSizeTypeId = pc.ProductSizeTypeId, ProductSizeTypeName = pc.ProductSizeType.Name, CreatedOn = pc.CreatedOn, UpdatedOn = pc.UpdatedOn, ProductGenders = pc.ProductGenders.Select(item => item.ProductGenderId).Distinct().ToList()  });


        return PagedList<ProductCategoryResponseDTO>.Create(productCategoriesMapped, page ,pagesize); 
    }

    public async Task<ProductCategoryResponseDTO> GetProductCategoryByIdAsync(Guid id)
    {
        if(id == Guid.Empty)
        {
            _logger.LogError("ProductCategoryServices::GetProductCategoryByIdAsync id is empty");
            throw new ArgumentNullException(nameof(id));
        }

        var productCategory = await _productCategoryRepository.GetCategoryWithSizeAsync(id);

        if(productCategory is null)
        {
            _logger.LogError("ProductCategoryServices::GetProductCategoryByIdAsync productCateogry is null");
            throw new ArgumentNullException(nameof(productCategory));
        }

        return new ProductCategoryResponseDTO 
        { 
            Id = productCategory.Id, 
            Name = productCategory.Name, 
            ProductSizeTypeId = productCategory.ProductSizeTypeId, 
            ProductSizeTypeName = productCategory.ProductSizeType.Name
        };
    }

    public async Task<bool> AddProductCategoryAsync(ProductCategoryRequestDTO productCategoryRequest)
    {
        if (productCategoryRequest is null)
        {
            _logger.LogError("ProductCategoryServices::AddProductCategoryAsync productCategoryRequest is null");
            throw new ArgumentNullException(nameof(productCategoryRequest));
        }

        await _validator.ValidateAndThrowAsync(productCategoryRequest);

        var categoryToAdd = new ProductCategory
        {
            Id = productCategoryRequest.Id ?? Guid.NewGuid(),
            Name = productCategoryRequest.Name.Trim(),
            ProductSizeTypeId = productCategoryRequest.ProductSizeTypeId,
        };

        var categoryAdded = await _productCategoryRepository.AddAsync(categoryToAdd);

        if (categoryAdded is null)
        {
            _logger.LogError("ProductCategoryServices::AddProductCategoryAsync categoryAdded is null");
            throw new ArgumentException(nameof(categoryAdded));
        }

        return await _unitOfWork.CommitAsync();
    }
    
    public async Task<ProductCategoryRequestDTO> BuildProductCategoryToUpdateAsync(Guid productCategoryId)
    {
        if(productCategoryId == Guid.Empty)
        {
            _logger.LogError("ProductCategoryServices::BuildProductCategoryToUpdateAsync  productCategoryId is Empty");
        }

        var category = await _productCategoryRepository.GetByIdAsync(productCategoryId);

        if(category is null)
        {
            _logger.LogError("ProductCategoryServices::BuildProductCategoryToUpdateAsync  category is Null");
            throw new ArgumentException(nameof(category));
        }

        var productcategoryRequestDTO = new ProductCategoryRequestDTO
        {
            Id = productCategoryId,
            Name = category.Name,
            ProductSizeTypeId = category.ProductSizeTypeId,
        };

        return productcategoryRequestDTO;
    }
    public async Task<bool> UpdateProductCategoryAsync(Guid id, ProductCategoryRequestDTO productCategoryRequest)
    {
        if(id == Guid.Empty)
        {
            _logger.LogError("ProductCategoryServices::UpdateProductCategoryAsync id is empty");
            throw new ArgumentNullException(nameof(id));
        }

        if (productCategoryRequest is null)
        {
            _logger.LogError("ProductCategoryServices::UpdateProductCategoryAsync productCategoryRequest is null");
            throw new ArgumentNullException(nameof(productCategoryRequest));
        }

        var categoryToUpdate = await _productCategoryRepository.GetByIdAsync(id);

        if(categoryToUpdate is null)
        {
            _logger.LogError("ProductCategoryServices::UpdateProductCategoryAsync categoryToUpdate is null");
            throw new ArgumentNullException(nameof(categoryToUpdate));
        }

        productCategoryRequest.Id = id;

        await _validator.ValidateAndThrowAsync(productCategoryRequest);

        categoryToUpdate.Name = productCategoryRequest.Name.Trim();
        categoryToUpdate.ProductSizeTypeId = productCategoryRequest.ProductSizeTypeId;

        return await _unitOfWork.UpdateAsync(categoryToUpdate);
         
    }

    public async Task<bool> RemoveProductCategoryAsync(Guid id)
    {
        if(id == Guid.Empty)
        {
            _logger.LogError("ProductCategoryServices::RemoveProductCategoryAsync id is empty");
            throw new ArgumentNullException(nameof(id));
        }

        var categoryToRemove = await _productCategoryRepository.GetByIdAsync(id);

        if( categoryToRemove is null)
        {
            _logger.LogError("ProductCategoryServices::UpdateProductCategoryAsync categoryToRemove is null");
            throw new ArgumentNullException(nameof(categoryToRemove));
        }

        _productCategoryRepository.Delete(categoryToRemove);
        return await _unitOfWork.CommitAsync();
    }
}
