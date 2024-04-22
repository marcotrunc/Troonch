
using FluentValidation;
using Microsoft.Extensions.Logging;
using Troonch.Application.Base.UnitOfWork;
using Troonch.Application.Base.Utilities;
using Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces;
using Troonch.RetailSales.Product.Domain.DTOs.Requests;
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
}
