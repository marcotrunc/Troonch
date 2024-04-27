
using FluentValidation;
using Microsoft.Extensions.Logging;
using Troonch.Application.Base.UnitOfWork;
using Troonch.Application.Base.Utilities;
using Troonch.DataAccess.Base.Helpers;
using Troonch.RetailSales.Product.DataAccess.Repositories;
using Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces;
using Troonch.RetailSales.Product.Domain.DTOs.Requests;
using Troonch.RetailSales.Product.Domain.DTOs.Responses;
using Troonch.Sales.Domain.Entities;
using SalesEntity = Troonch.Sales.Domain.Entities;

namespace Troonch.RetailSales.Product.Application.Services;

public class ProductServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;
    private readonly ILogger<ProductServices> _logger;
    private readonly IValidator<ProductRequestDTO> _validator;

    public ProductServices(IUnitOfWork unitOfWork, IProductRepository productRepository, ILogger<ProductServices> logger, IValidator<ProductRequestDTO> validator)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _logger = logger;
        _validator = validator;
    }

    public async Task<PagedList<ProductResponseDTO>> GetProductsAsync(string? searchTerm, int page = 1, int pageSize = 10)
    {
        var products = await _productRepository.GetProductsAsync(searchTerm);

        if (products is null)
        {
            _logger.LogError("ProductServices::GetProductsAsync products is null");
            throw new ArgumentNullException(nameof(products));
        }

        var productsMapped = mapFromProductsToProductsResponseDTO(products);

        var productsPaged =  PagedList<ProductResponseDTO>.Create(productsMapped, page, pageSize);

        return productsPaged;
    }
    public async Task<ProductResponseDTO> GetProductByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            _logger.LogError("ProductCategoryServices::GetProductByIdAsync id is empty");
            throw new ArgumentNullException(nameof(id));
        }

        var product= await _productRepository.GetProductByIdAsync(id);

        if (product is null)
        {
            _logger.LogError("ProductCategoryServices::GetProductByIdAsync product is null");
            throw new ArgumentNullException(nameof(product));
        }

        return mapFromProductToProductResponseDTO(product);
    }
    public async Task<bool> AddProductAsync(ProductRequestDTO productRequest)
    {
        if(productRequest == null)
        {
            _logger.LogError("ProductServices::AddProductAsync productRequest is null");
            throw new ArgumentNullException(nameof(productRequest));
        }

        await _validator.ValidateAndThrowAsync(productRequest);

        var productToAdd = new SalesEntity.Product()
        {
            Name = productRequest.Name.Trim(),
            Description = productRequest.Description,
            Slug = SlugUtility.GenerateSlug(productRequest.Name.Trim()),
            IsDeleted = false,
            IsPublished = productRequest.IsPublished,
            CoverImageLink = productRequest.CoverImageLink,
            ProductGenderId = productRequest.ProductGenderId,
            ProductBrandId = productRequest.ProductBrandId,
            ProductCategoryId = productRequest.ProductCategoryId,
            ProductMaterialId = productRequest.ProductMaterialId == Guid.Empty ? null : productRequest.ProductMaterialId,
        };

        //TODO Implement the saving 

        var productAdded = await _productRepository.AddAsync(productToAdd); 

        if(productAdded is null)
        {
            _logger.LogError("ProductServices::AddProductAsync productAdded is null");
            throw new ArgumentNullException(nameof(productAdded));
        }

        return await _unitOfWork.CommitAsync();
    }

    public async Task<bool> UpdateProductAsync(Guid id, ProductRequestDTO productRequest)
    {
        if (id == Guid.Empty)
        {
            _logger.LogError("ProductServices::UpdateProductAsync id is empty");
            throw new ArgumentNullException(nameof(id));
        }

        productRequest.Id = id;

        if (productRequest is null)
        {
            _logger.LogError("ProductServices::UpdateProductAsync productRequest is null");
            throw new ArgumentNullException(nameof(productRequest));
        }

        var productToUpdate = await _productRepository.GetByIdAsync(id);

        if (productToUpdate is null)
        {
            _logger.LogError("ProductServices::UpdateProductAsync productToUpdate is null");
            throw new ArgumentNullException(nameof(productToUpdate));
        }

        await _validator.ValidateAndThrowAsync(productRequest);

        productToUpdate.Name = productRequest.Name.Trim();
        productToUpdate.Description = productRequest.Description;
        productToUpdate.Slug = SlugUtility.GenerateSlug(productRequest.Name.Trim());
        productToUpdate.IsDeleted = productRequest.IsDeleted;
        productToUpdate.IsPublished = productRequest.IsPublished;
        productToUpdate.CoverImageLink = productToUpdate.CoverImageLink;
        productToUpdate.ProductGenderId = productRequest.ProductGenderId;
        productToUpdate.ProductBrandId = productRequest.ProductBrandId;
        productToUpdate.ProductCategoryId = productRequest.ProductCategoryId;
        productToUpdate.ProductMaterialId = productRequest.ProductMaterialId;

        return await _unitOfWork.UpdateAsync(productToUpdate);

    }

    public async Task<bool> DoSoftDeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            _logger.LogError("ProductServices::DoSoftDeleteAsync id is empty");
            throw new ArgumentNullException(nameof(id));
        }

        var productToDelete = await _productRepository.GetByIdAsync(id);

        if (productToDelete is null)
        {
            _logger.LogError("ProductServices::DoSoftDeleteAsync productToDelete is null");
            throw new ArgumentNullException(nameof(productToDelete));
        }

        productToDelete.IsDeleted = false;
        
        return await _unitOfWork.UpdateAsync(productToDelete);

    }

    public async Task<bool> RemoveProductAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            _logger.LogError("ProductServices::RemoveProductAsync id is empty");
            throw new ArgumentNullException(nameof(id));
        }

        var productToRemove = await _productRepository.GetByIdAsync(id);

        if (productToRemove is null)
        {
            _logger.LogError("ProductServices::RemoveProductAsync productToRemove is null");
            throw new ArgumentNullException(nameof(productToRemove));
        }

        _productRepository.Delete(productToRemove);
        return await _unitOfWork.CommitAsync();
    }


    #region Ecommerce Product Services

    public async Task<PagedList<ProductResponseDTO>> GetProductsPublishedAsync(string? searchTerm, int page = 1, int pageSize = 10)
    {
        var products = await _productRepository.GetProductsPublishedAsync(searchTerm);

        if (products is null)
        {
            _logger.LogError("ProductServices::GetProductsPublishedAsync products is null");
            throw new ArgumentNullException(nameof(products));
        }

        var productsMapped = mapFromProductsToProductsResponseDTO(products);

        var productsPaged = PagedList<ProductResponseDTO>.Create(productsMapped, page, pageSize);

        return productsPaged;
    }

    public async Task<ProductResponseDTO> GetProductPublishedByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            _logger.LogError("ProductCategoryServices::GetProductPublishedByIdAsync id is empty");
            throw new ArgumentNullException(nameof(id));
        }

        var product = await _productRepository.GetProductPublishedByIdAsync(id);

        if (product is null)
        {
            _logger.LogError("ProductCategoryServices::GetProductPublishedByIdAsync product is null");
            throw new ArgumentNullException(nameof(product));
        }

        return mapFromProductToProductResponseDTO(product);
    }

    public async Task<PagedList<ProductResponseDTO>> GetProductsPublishedByCategoryIdAsync(Guid categoryId, int page = 1, int pageSize = 10)
    {
        if (categoryId == Guid.Empty)
        {
            _logger.LogError("ProductCategoryServices::GetProductsPublishedByCategoryIdAsync categoryId is empty");
            throw new ArgumentNullException(nameof(categoryId));
        }

        var products = await _productRepository.GetProductsPublishedByCategoryIdAsync(categoryId);

        if (products is null)
        {
            _logger.LogError("ProductCategoryServices::GetProductsPublishedByCategoryIdAsync products is null");
            throw new ArgumentNullException(nameof(products));
        }

        var productsMapped = mapFromProductsToProductsResponseDTO(products);

        var productsPaged = PagedList<ProductResponseDTO>.Create(productsMapped, page, pageSize);

        return productsPaged;
    }

    public async Task<PagedList<ProductResponseDTO>> GetProductsPublishedByBrandIdAsync(Guid brandId, int page = 1, int pageSize = 10)
    {
        if (brandId == Guid.Empty)
        {
            _logger.LogError("ProductCategoryServices::GetProductsPublishedByBrandIdAsync brandId is empty");
            throw new ArgumentNullException(nameof(brandId));
        }

        var products = await _productRepository.GetProductsPublishedByBrandIdAsync(brandId);

        if (products is null)
        {
            _logger.LogError("ProductCategoryServices::GetProductsPublishedByBrandIdAsync products is null");
            throw new ArgumentNullException(nameof(products));
        }

        var productsMapped = mapFromProductsToProductsResponseDTO(products);

        var productsPaged = PagedList<ProductResponseDTO>.Create(productsMapped, page, pageSize);

        return productsPaged;
    }
    public async Task<PagedList<ProductResponseDTO>> GetProductsPublishedByGenderIdAsync(Guid genderId, int page = 1, int pageSize = 10)
    {
        if (genderId == Guid.Empty)
        {
            _logger.LogError("ProductCategoryServices::GetProductsPublishedByGenderIdAsync genderId is empty");
            throw new ArgumentNullException(nameof(genderId));
        }

        var products = await _productRepository.GetProductsPublishedByBrandIdAsync(genderId);

        if (products is null)
        {
            _logger.LogError("ProductCategoryServices::GetProductsPublishedByGenderIdAsync products is null");
            throw new ArgumentNullException(nameof(products));
        }

        var productsMapped = mapFromProductsToProductsResponseDTO(products);

        var productsPaged = PagedList<ProductResponseDTO>.Create(productsMapped, page, pageSize);

        return productsPaged;
    }
    #endregion
    private IEnumerable<ProductResponseDTO> mapFromProductsToProductsResponseDTO(IEnumerable<SalesEntity.Product> productsToMap) =>
            productsToMap.Select(p => new ProductResponseDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Slug = p.Slug,
                IsDeleted = p.IsDeleted,
                IsPublished = p.IsPublished,
                CoverImageLink = p.CoverImageLink,
                ProductGenderId = p.ProductGenderId,
                ProductGenderName = p.ProductGender.Name,
                ProductBrandId = p.ProductBrandId,
                ProductBrandName = p.ProductBrand.Name,
                ProductCategoryId = p.ProductCategoryId,
                ProductCategoryName = p.ProductCategory.Name,
                ProductMaterialId = p.ProductMaterialId,
                ProductMaterialName = p.ProductMaterial?.Value ?? null
            });

    private ProductResponseDTO mapFromProductToProductResponseDTO(SalesEntity.Product p) => 
            new ProductResponseDTO 
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Slug = p.Slug,
                IsDeleted = p.IsDeleted,
                IsPublished = p.IsPublished,
                CoverImageLink = p.CoverImageLink,
                ProductGenderId = p.ProductGenderId,
                ProductGenderName = p.ProductGender.Name,
                ProductBrandId = p.ProductBrandId,
                ProductBrandName = p.ProductBrand.Name,
                ProductCategoryId = p.ProductCategoryId,
                ProductCategoryName = p.ProductCategory.Name,
                ProductMaterialId = p.ProductMaterialId,
                ProductMaterialName = p.ProductMaterial?.Value ?? null
            };

}
