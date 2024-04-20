using FluentValidation;
using Troonch.RetailSales.Product.DataAccess.Repositories;
using Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces;
using Troonch.RetailSales.Product.Domain.DTOs.Requests;

namespace Troonch.RetailSales.Product.Application.Validators;

public class ProductCategoryReqValidator : AbstractValidator<ProductCategoryRequestDTO>
{
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly IProductSizeTypeRepository _productSizeTypeRepository;
    public ProductCategoryReqValidator(IProductCategoryRepository productCategoryRepository, IProductSizeTypeRepository productSizeTypeRepository)
    {
        _productCategoryRepository = productCategoryRepository;
        _productSizeTypeRepository = productSizeTypeRepository;

        RuleFor(pb => pb.Name)
            .NotEmpty().WithMessage("dsa")
            .MaximumLength(128).WithMessage("das")
            .MinimumLength(1).WithMessage("dasa")
            .NotNull().WithMessage("dsa")
            .MustAsync(async (b, name, _) => await _productCategoryRepository.IsUniqueNameAsync(name)).When(pc => pc.Id == Guid.Empty);

        RuleFor(pb => pb.ProductSizeTypeId)
            .NotNull()
            .NotEmpty()
            .MustAsync(async (b, ProductSizeTypeId, _) => await _productSizeTypeRepository.Exists(ProductSizeTypeId)).WithMessage("The size type not exists");
    
    
    }
}
