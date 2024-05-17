using FluentValidation;
using System.Globalization;
using System.Text;
using Troonch.RetailSales.Product.Application.Validators.Resources;
using Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces;
using Troonch.RetailSales.Product.Domain.DTOs.Requests;

namespace Troonch.RetailSales.Product.Application.Validators
{
    public class ProductReqValidator : AbstractValidator<ProductRequestDTO>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductBrandRepository _productBrandRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IProductGenderRepository _productGenderRepository;
        private readonly IProductMaterialRepository _productMaterialRepository;
        private readonly ResourcesHelper _resourceHelper;



        public ProductReqValidator(
            IProductRepository productRepository,
            IProductBrandRepository productBrandRepository,
            IProductCategoryRepository productCategoryRepository,
            IProductGenderRepository productGenderRepository,
            IProductMaterialRepository productMaterialRepository,
            ResourcesHelper resourceHelper
            )
        {
            _productRepository = productRepository;
            _productBrandRepository = productBrandRepository;
            _productCategoryRepository = productCategoryRepository;
            _productGenderRepository = productGenderRepository;
            _productMaterialRepository = productMaterialRepository;
            _resourceHelper = resourceHelper;


            RuleFor(p => p.Name)
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
                .MustAsync(async (b, name, _) => await _productRepository.IsNameUniqueAync(b.Id, name))
                    .WithMessage(_resourceHelper.GetString("UNIQUE_FIELD_ERROR", new List<ResourceHelperParameter>
                    {
                        new ResourceHelperParameter { ParameterKey = "NAME_TRANSLATE" },
                    }));


            RuleFor(p => p.Description)
                .MinimumLength(1).When(p => !String.IsNullOrWhiteSpace(p.Description))
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

            RuleFor(p => p.ProductGenderId)
                .NotNull()
                    .WithMessage(_resourceHelper.GetString("NULL_FIELD_ERROR", new List<ResourceHelperParameter>
                    {
                        new ResourceHelperParameter {ParameterKey = "PRODUCT_GENDER_TRANSLATE"}
                    }))
                .NotEmpty()
                    .WithMessage(_resourceHelper.GetString("EMPTY_FIELD_ERROR", new List<ResourceHelperParameter> {
                        new ResourceHelperParameter {ParameterKey =  "PRODUCT_GENDER_TRANSLATE" }
                    }))
                .MustAsync(async (b, productGenderId, _) => await _productGenderRepository.IsExistingById(productGenderId))
                    .WithMessage(_resourceHelper.GetString("EXISTING_FIELD_ERROR", new List<ResourceHelperParameter> {
                        new ResourceHelperParameter {ParameterKey =  "PRODUCT_GENDER_TRANSLATE" }
                    }));

            RuleFor(p => p.ProductBrandId)
                .NotNull()
                    .WithMessage(_resourceHelper.GetString("NULL_FIELD_ERROR", new List<ResourceHelperParameter>
                    {
                        new ResourceHelperParameter {ParameterKey = "PRODUCT_BRAND_TRANSLATE"}
                    }))
                .NotEmpty()
                    .WithMessage(_resourceHelper.GetString("EMPTY_FIELD_ERROR", new List<ResourceHelperParameter> {
                        new ResourceHelperParameter {ParameterKey =  "PRODUCT_BRAND_TRANSLATE" }
                    }))
                .MustAsync(async (b, productBrandId, _) => await _productBrandRepository.IsExistingById(productBrandId))
                    .WithMessage(_resourceHelper.GetString("EXISTING_FIELD_ERROR", new List<ResourceHelperParameter> {
                        new ResourceHelperParameter {ParameterKey =  "PRODUCT_BRAND_TRANSLATE" }
                    }));

            RuleFor(p => p.ProductCategoryId)
                .NotNull()
                    .WithMessage(_resourceHelper.GetString("NULL_FIELD_ERROR", new List<ResourceHelperParameter>
                    {
                        new ResourceHelperParameter {ParameterKey = "PRODUCT_CATEGORY_TRANSLATE"}
                    }))
                .NotEmpty()
                    .WithMessage(_resourceHelper.GetString("EMPTY_FIELD_ERROR", new List<ResourceHelperParameter> {
                        new ResourceHelperParameter {ParameterKey =  "PRODUCT_CATEGORY_TRANSLATE" }
                    }))
                .MustAsync(async (b, productCategoryId, _) => await _productCategoryRepository.IsExistingById(productCategoryId))
                    .WithMessage(_resourceHelper.GetString("EXISTING_FIELD_ERROR", new List<ResourceHelperParameter> {
                        new ResourceHelperParameter {ParameterKey =  "PRODUCT_CATEGORY_TRANSLATE" }
                    }));

            RuleFor(p => p.ProductMaterialId)
                .NotNull()
                    .WithMessage(_resourceHelper.GetString("NULL_FIELD_ERROR", new List<ResourceHelperParameter>
                    {
                        new ResourceHelperParameter {ParameterKey = "PRODUCT_MATERIAL_TRANSLATE"}
                    }))
                .NotEmpty()
                    .WithMessage(_resourceHelper.GetString("EMPTY_FIELD_ERROR", new List<ResourceHelperParameter> {
                        new ResourceHelperParameter {ParameterKey =  "PRODUCT_MATERIAL_TRANSLATE" }
                    }))
                .MustAsync(async (b, productMaterialId, _) => await _productMaterialRepository.IsExistingById(productMaterialId))
                    .When(p => p.ProductMaterialId != Guid.Empty)
                    .WithMessage(_resourceHelper.GetString("EXISTING_FIELD_ERROR", new List<ResourceHelperParameter> {
                        new ResourceHelperParameter {ParameterKey =  "PRODUCT_MATERIAL_TRANSLATE" }
                    }));
        }
    }
}
