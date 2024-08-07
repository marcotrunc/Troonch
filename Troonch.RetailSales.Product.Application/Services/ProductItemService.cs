﻿using FluentValidation;
using Microsoft.Extensions.Logging;
using System.Globalization;
using Troonch.Application.Base.UnitOfWork;
using Troonch.Application.Base.Utilities;
using Troonch.DataAccess.Base.Helpers;
using Troonch.Domain.Base.Enums;
using Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces;
using Troonch.RetailSales.Product.Domain.DTOs.Requests;
using Troonch.RetailSales.Product.Domain.DTOs.Responses;
using Troonch.Sales.Domain.Entities;

namespace Troonch.RetailSales.Product.Application.Services;

public class ProductItemService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductItemRepository _productItemRepository;
    private readonly ILogger<ProductItemService> _logger;
    private readonly IValidator<ProductItemRequestDTO> _validator;

    public ProductItemService(
        IUnitOfWork unitOfWork,
        IProductItemRepository productItemRepository, 
        ILogger<ProductItemService> logger, 
        IValidator<ProductItemRequestDTO> validator
    )
    {
        _unitOfWork = unitOfWork;
        _productItemRepository = productItemRepository;
        _logger = logger;
        _validator = validator;
    }

    public async Task<IEnumerable<ProductItemResponse>> GetProductItemsByProductIdAsync(Guid productId)
    {
        if(productId == Guid.Empty) 
        {
            _logger.LogError("ProductItemService::GetProductItemsByProductIdAsync productId is guid empty");
            throw new ArgumentNullException(nameof(productId));
        }

        var productItems = await _productItemRepository.GetProductItemsByProductIdAsync(productId);

        if(productItems is null) 
        {
            _logger.LogError("ProductItemService::GetProductItemsByProductIdAsync productItems is null");
            throw new ArgumentNullException(nameof(productItems));
        }

        return MapProductItemsToProductItemResponse(productItems);
    }

    public async Task<bool> AddProductItemAsync(ProductItemRequestDTO productItemRequest)
    {
        if (productItemRequest is null)
        {
            _logger.LogError("ProductItemService::AddProductItemAsync productItemRequest is null");
            throw new ArgumentNullException(nameof(productItemRequest));
        }

        await _validator.ValidateAndThrowAsync(productItemRequest);

        var productItemToAdd = new ProductItem
        {
            ProductSizeOptionId = productItemRequest.ProductSizeOptionId,
            Color = productItemRequest.ProductColor,
            OriginalPrice = decimal.TryParse(productItemRequest.OriginalPrice, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal originalPriceParsed) ? originalPriceParsed : throw new FormatException(nameof(productItemRequest.OriginalPrice)),
            ProductId = productItemRequest.ProductId,
            //TODO
            //Currency =  get Currency fron config table
            SalePrice = decimal.TryParse(productItemRequest.SalePrice, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal salePriceParsed) ? salePriceParsed : throw new FormatException(nameof(productItemRequest.SalePrice)),
            Barcode = productItemRequest.Barcode, 
            QuantityAvailable = productItemRequest.QuantityAvailable,
        };

        var productItemAdded = await _productItemRepository.AddAsync(productItemToAdd);

        if (productItemAdded is null)
        {
            _logger.LogError("ProductItemService::AddProductItemAsync productItemAdded is null");
            throw new ArgumentNullException(nameof(productItemAdded));
        }

        return await _unitOfWork.CommitAsync();
    }

    public async Task<ProductItemRequestDTO> GetProductByIdForUpdateAsync(Guid productItemId)
    {
        if (productItemId == Guid.Empty)
        {
            _logger.LogError("ProductItemService::GetProductByIdForUpdateAsync productItemId is empty");
            throw new ArgumentNullException(nameof(productItemId));
        }

        var productItem = await _productItemRepository.GetByIdAsync(productItemId);

        if(productItem is null)
        {
            _logger.LogError("ProductItemService::GetProductByIdForUpdateAsync productItem is null");
            throw new ArgumentNullException(nameof(productItem));
        }

        return MapFromProductItemToItemRequest(productItem);
    }

    public async Task<bool> UpdateProductItemAsync(Guid productItemId, ProductItemRequestDTO productItemRequest)
    {
        if (productItemId == Guid.Empty)
        {
            _logger.LogError("ProductItemService::UpdateProductItemAsync id is empty");
            throw new ArgumentNullException(nameof(productItemId));
        }

        if (productItemRequest is null)
        {
            _logger.LogError("ProductItemService::UpdateProductItemAsync productItemRequest is null");
            throw new ArgumentNullException(nameof(productItemRequest));
        }

        var productItemToUpdate = await _productItemRepository.GetByIdAsync(productItemId);

        if (productItemToUpdate is null)
        {
            _logger.LogError("ProductItemService::UpdateProductItemAsync productItemToUpdate is null");
            throw new ArgumentNullException(nameof(productItemToUpdate));
        }

        await _validator.ValidateAndThrowAsync(productItemRequest);

        productItemToUpdate.ProductSizeOptionId = productItemRequest.ProductSizeOptionId;
        productItemToUpdate.Color = productItemRequest.ProductColor;
        productItemToUpdate.OriginalPrice = decimal.TryParse(productItemRequest.OriginalPrice, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal originalPriceParsed) ? originalPriceParsed : throw new FormatException(nameof(productItemRequest.OriginalPrice));
        productItemToUpdate.ProductId = productItemRequest.ProductId;
        //TODO
        //productItemToUpdate.Currency =  get Currency fron config table
        productItemToUpdate.SalePrice = decimal.TryParse(productItemRequest.SalePrice, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal salePriceParsed) ? salePriceParsed : throw new FormatException(nameof(productItemRequest.SalePrice));
        productItemToUpdate.Barcode = productItemRequest.Barcode;
        productItemToUpdate.QuantityAvailable = productItemRequest.QuantityAvailable;

        return await  _unitOfWork.UpdateAsync(productItemToUpdate);

    }
    public async Task<bool> DeleteProductItemByIdAsync(Guid productItemId)
    {
        if (productItemId == Guid.Empty)
        {
            _logger.LogError("ProductItemService::DeleteProductItemByIdAsync productItemId is empty");
            throw new ArgumentNullException(nameof(productItemId));
        }

        var productItemToDelete = await _productItemRepository.GetByIdAsync(productItemId);

        if (productItemToDelete is null)
        {
            _logger.LogError("ProductItemService::DeleteProductItemByIdAsync productItemToDelete is null");
            throw new ArgumentNullException(nameof(productItemToDelete));
        }

        _productItemRepository.Delete(productItemToDelete);

        return await _unitOfWork.CommitAsync();
    }

    private ProductItemRequestDTO MapFromProductItemToItemRequest(ProductItem productItem) => new ProductItemRequestDTO()
    {
        Id = productItem.Id,
        ProductId = productItem.ProductId,
        ProductSizeOptionId = productItem.ProductSizeOptionId,
        ProductColor = productItem.Color,
        Currency = productItem.Currency,
        OriginalPrice = Convert.ToString(productItem.OriginalPrice, CultureInfo.InvariantCulture),
        SalePrice = Convert.ToString(productItem.SalePrice, CultureInfo.InvariantCulture),
        Barcode = productItem.Barcode,
        QuantityAvailable = productItem.QuantityAvailable,
    };
    
    private IEnumerable<ProductItemResponse> MapProductItemsToProductItemResponse(IEnumerable<ProductItem> productItems)
    {
        var response = new List<ProductItemResponse>();

        foreach (var productItem in productItems)
        {
            var item = new ProductItemResponse()
            {
                Id = productItem.Id,    
                ProductId = productItem.ProductId,
                OriginalPrice = productItem.OriginalPrice,
                SalePrice= productItem.SalePrice,
                Barcode= productItem.Barcode,
                QuantityAvailable = productItem.QuantityAvailable,
                Currency = productItem.Currency,
                ColorName = productItem.Color,
                Size = productItem.ProductSizeOption.Value ?? null,
            };

            response.Add(item);
        }

        return response;
    }
    
}
