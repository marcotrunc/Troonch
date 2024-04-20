using FluentValidation;
using Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces;
using Troonch.RetailSales.Product.Domain.DTOs.Requests;

namespace Troonch.RetailSales.Product.Application.Validators;

public class ProductBrandReqValidator :  AbstractValidator<ProductBrandRequestDTO>
{
    private readonly IProductBrandRepository _productBrandRepository;
    public ProductBrandReqValidator(IProductBrandRepository productBrandRepository)
    {
        _productBrandRepository = productBrandRepository;

        RuleFor(pb => pb.Name)
            .NotEmpty().WithMessage("dsa")
            .MaximumLength(128).WithMessage("das")
            .MinimumLength(1).WithMessage("dasa")
            .NotNull().WithMessage("dsa")
            .MustAsync(async (b, name, _) => await _productBrandRepository.IsUniqueNameAsync(name));

        RuleFor(pb => pb.Description)
            .MaximumLength(256).WithMessage("dsa");
            
    }

}
