using FluentValidation;
using System.Globalization;
using Troonch.RetailSales.Product.Application.Validators.Resources;
using Troonch.RetailSales.Product.DataAccess.Repositories;
using Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces;
using Troonch.RetailSales.Product.Domain.DTOs.Requests;

namespace Troonch.RetailSales.Product.Application.Validators;

public class ProductItemReqValidator : AbstractValidator<ProductItemRequestDTO>
{
    private readonly IProductItemRepository _productItemRepository;
    private readonly IProductRepository _productRepository;
    private readonly IProductColorRepository _productColorRepository;
    private readonly ResourcesHelper _resourcesHelper;
    
    public ProductItemReqValidator(
        IProductItemRepository productItemRepository,
        IProductRepository productRepository,
        IProductColorRepository productColorRepository,
        ResourcesHelper resourcesHelper
        )
    {
        _productItemRepository = productItemRepository;
        _productRepository = productRepository;
        _productColorRepository = productColorRepository;
        _resourcesHelper = resourcesHelper;

        RuleFor(pi => pi.ProductId)
            .NotNull()
                .WithMessage("da")
            .NotEmpty()
                .WithMessage("da")
            .MustAsync(async (b, productId, _) => await _productRepository.IsExistingById(productId))
                .WithMessage("dsa");

        RuleFor(pi => pi.ProductColorId)
            .NotNull()
                .WithMessage("da")
            .NotEmpty()
                .WithMessage("da")
            .MustAsync(async (b, productColorId, _) => await _productColorRepository.IsExistingById(productColorId))
                .WithMessage("dsa");

        RuleFor(pi => pi.Barcode)
            .NotEmpty()
                .WithMessage("String cannot be empty.")
            .NotNull()
                .WithMessage("String must not be null.")
            .MustAsync(async (b, barcode, _) => await _productItemRepository.IsUniqueBarcodeAsync(b.Id, barcode))
                .WithMessage("The barcode can be unique");

        RuleFor(pi => pi.SalePrice)
            .NotEmpty()
                .WithMessage("Must contain an value")
             .Matches("^(1000000000000000000000000000000000(\\.0{1,2})?|([1-9]\\d{0,29}|0)(\\.\\d{1,2})?|\\.\\d{1,2})$")
                .WithMessage("The input must be a valid decimal number.")
            .Must(salePrice => decimal.TryParse(salePrice, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal result) && result > 0)
                .WithMessage("Money value must be greater than zero.");

        RuleFor(pi => pi.OriginalPrice)
            .Matches("^(1000000000000000000000000000000000(\\.0{1,2})?|([1-9]\\d{0,29}|0)(\\.\\d{1,2})?|\\.\\d{1,2})$")
                .WithMessage("The input must be a valid decimal number.")
            .Must(originalPrice => decimal.TryParse(originalPrice, out decimal result) && result >= 0)
                .WithMessage("The input must be greater than or equal to 0.");

        RuleFor(pi => pi.QuantityAvailable)
            .GreaterThanOrEqualTo(0)
                .WithMessage("Value must be greater or equal than zero.");

    }
}
