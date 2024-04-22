using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public ProductReqValidator(
            IProductRepository productRepository,
            IProductBrandRepository productBrandRepository,
            IProductCategoryRepository productCategoryRepository,
            IProductGenderRepository productGenderRepository,
            IProductMaterialRepository productMaterialRepository
            )
        {
            _productRepository = productRepository;
            _productBrandRepository = productBrandRepository;
            _productCategoryRepository = productCategoryRepository;
            _productGenderRepository = productGenderRepository;
            _productMaterialRepository = productMaterialRepository;


            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("da")
                .MaximumLength(128).WithMessage("das")
                .MinimumLength(1).WithMessage("dasa")
                .NotNull().WithMessage("dsa")
                .MustAsync(async (b, name, _) => await _productRepository.IsNameUniqueAync(name)).When(pc => pc.Id == Guid.Empty).WithMessage("da");
            
            RuleFor(p => p.Description)
                .MinimumLength(1).When(p => p.Description is not null).WithMessage("dsa")
                .MaximumLength(256).When(p => p.Description is not null).WithMessage("dsa");

            RuleFor(p => p.ProductGenderId)
                .NotNull()
                .NotEmpty()
                .MustAsync(async (b, producrGenderId, _) => await _productGenderRepository.IsExistingById(producrGenderId))
                .WithMessage("dsa");

            RuleFor(p => p.ProductBrandId)
                .NotNull()
                .NotEmpty()
                .MustAsync(async (b, productBrandId, _) => await _productBrandRepository.IsExistingById(productBrandId))
                .WithMessage("dsa");

            RuleFor(p => p.ProductCategoryId)
                .NotNull()
                .NotEmpty()
                .MustAsync(async (b, productCategoryId, _) => await _productCategoryRepository.IsExistingById(productCategoryId))
                .WithMessage("dsa");

            RuleFor(p => p.ProductMaterialId)
                .NotNull()
                .NotEmpty()
                .MustAsync(async (b, productMaterialId, _) => await _productMaterialRepository.IsExistingById(productMaterialId))
                .When(p => p.ProductMaterialId != Guid.Empty)
                .WithMessage("dsa");
        }
    }
}
