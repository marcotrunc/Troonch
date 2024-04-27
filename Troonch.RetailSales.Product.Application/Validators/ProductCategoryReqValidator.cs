using FluentValidation;
using Troonch.RetailSales.Product.Application.Validators.Resources;
using Troonch.RetailSales.Product.DataAccess.Repositories;
using Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces;
using Troonch.RetailSales.Product.Domain.DTOs.Requests;

namespace Troonch.RetailSales.Product.Application.Validators;

public class ProductCategoryReqValidator : AbstractValidator<ProductCategoryRequestDTO>
{
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly IProductSizeTypeRepository _productSizeTypeRepository;
    private readonly ResourcesHelper _resourceHelper;
    public ProductCategoryReqValidator(
        IProductCategoryRepository productCategoryRepository, 
        IProductSizeTypeRepository productSizeTypeRepository,
        ResourcesHelper resourceHelper
        )
    {
        _productCategoryRepository = productCategoryRepository;
        _productSizeTypeRepository = productSizeTypeRepository;
        _resourceHelper = resourceHelper;

        RuleFor(pb => pb.Name)
            .NotEmpty()
                .WithMessage(_resourceHelper.GetString("EMPTY_FIELD_ERROR", new List<ResourceHelperParameter> {
                    new ResourceHelperParameter {ParameterKey =  "NAME_TRANSLATE" }
                }))
            .MaximumLength(128)
                .WithMessage(_resourceHelper.GetString("MAXIMUM_LENGTH_ERROR", new List<ResourceHelperParameter> {
                    new ResourceHelperParameter { ParameterKey = "NAME_TRANSLATE" },
                    new ResourceHelperParameter { ParameterKey = "128", IsInResource= false}
                }))
                .MinimumLength(1).WithMessage(_resourceHelper.GetString("MINIMUM_LENGTH_ERROR", new List<ResourceHelperParameter> {
                    new ResourceHelperParameter { ParameterKey = "NAME_TRANSLATE" },
                    new  ResourceHelperParameter {ParameterKey = "1", IsInResource = false}
                }))
            .NotNull()
                .WithMessage(_resourceHelper.GetString("NULL_FIELD_ERROR", new List<ResourceHelperParameter>
                {
                    new ResourceHelperParameter { ParameterKey = "NAME_TRANSLATE" },
                }))
            .MustAsync(async (b, name, _) => await _productCategoryRepository.IsUniqueNameAsync(b.Id, name))
                .WithMessage(_resourceHelper.GetString("UNIQUE_FIELD_ERROR", new List<ResourceHelperParameter>
                {
                    new ResourceHelperParameter { ParameterKey = "NAME_TRANSLATE" },
                }));

        RuleFor(pb => pb.ProductSizeTypeId)
            .NotNull()
                .WithMessage(_resourceHelper.GetString("NULL_FIELD_ERROR", new List<ResourceHelperParameter>
                {
                    new ResourceHelperParameter { ParameterKey = "PRODUCT_SITE_TYPE_TRANSLATE" },
                }))
            .NotEmpty()
                .WithMessage(_resourceHelper.GetString("EMPTY_FIELD_ERROR", new List<ResourceHelperParameter> {
                    new ResourceHelperParameter {ParameterKey =  "PRODUCT_SITE_TYPE_TRANSLATE" }
                }))
            .MustAsync(async (b, ProductSizeTypeId, _) => await _productSizeTypeRepository.Exists(ProductSizeTypeId))
                .WithMessage(_resourceHelper.GetString("EXISTING_FIELD_ERROR", new List<ResourceHelperParameter> {
                        new ResourceHelperParameter {ParameterKey =  "PRODUCT_SITE_TYPE_TRANSLATE" }
                }));


    }
}
