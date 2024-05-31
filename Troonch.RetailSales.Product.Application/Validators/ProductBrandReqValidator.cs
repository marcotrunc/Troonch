using FluentValidation;
using Troonch.RetailSales.Product.Application.Validators.Resources;
using Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces;
using Troonch.RetailSales.Product.Domain.DTOs.Requests;

namespace Troonch.RetailSales.Product.Application.Validators;

public class ProductBrandReqValidator :  AbstractValidator<ProductBrandRequestDTO>
{
    private readonly IProductBrandRepository _productBrandRepository;
    private readonly ResourcesHelper _resourceHelper;
    public ProductBrandReqValidator(
        IProductBrandRepository productBrandRepository,
        ResourcesHelper resourceHelper
        )
    {
        _productBrandRepository = productBrandRepository;
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
            .MinimumLength(1)
                    .WithMessage(_resourceHelper.GetString("MINIMUM_LENGTH_ERROR", new List<ResourceHelperParameter> {
                        new ResourceHelperParameter { ParameterKey = "NAME_TRANSLATE" },
                        new  ResourceHelperParameter {ParameterKey = "1", IsInResource = false}
                    }))
            .NotNull()
                    .WithMessage(_resourceHelper.GetString("NULL_FIELD_ERROR", new List<ResourceHelperParameter>
                    {
                        new ResourceHelperParameter { ParameterKey = "NAME_TRANSLATE" },
                    }))
            .MustAsync(async (b, name, _) => await _productBrandRepository.IsUniqueNameAsync(b.Id,name))
                .WithMessage(_resourceHelper.GetString("UNIQUE_FIELD_ERROR", new List<ResourceHelperParameter>
                    {
                        new ResourceHelperParameter { ParameterKey = "NAME_TRANSLATE" },
                    }));
        

            RuleFor(pb => pb.Description)
                .MinimumLength(2).When(p => !String.IsNullOrWhiteSpace(p.Description))
                    .WithMessage(_resourceHelper.GetString("MINIMUM_LENGTH_ERROR", new List<ResourceHelperParameter>
                    {
                        new ResourceHelperParameter {ParameterKey = "DESCRIPTION_TRANSLATE"},
                        new ResourceHelperParameter {ParameterKey = "1", IsInResource = false},
                    }))
                .MaximumLength(256).When(p => !String.IsNullOrWhiteSpace(p.Description))
                    .WithMessage(_resourceHelper.GetString("MAXIMUM_LENGTH_ERROR", new List<ResourceHelperParameter>
                    {
                        new ResourceHelperParameter {ParameterKey = "DESCRIPTION_TRANSLATE"},
                        new ResourceHelperParameter {ParameterKey = "256", IsInResource = false},
                    }));

    }

}
