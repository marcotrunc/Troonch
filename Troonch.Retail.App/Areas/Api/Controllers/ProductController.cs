using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Troonch.Application.Base.Utilities;
using Troonch.Domain.Base.DTOs.Response;
using Troonch.RetailSales.Product.Application.Services;
using Troonch.RetailSales.Product.Domain.DTOs.Requests;

namespace Troonch.Retail.App.Areas.Api.Controllers
{
    [Area("Api")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly ProductServices _productService;
        private readonly ProductBrandService _brandService;
        private readonly ProductCategoryServices _categoryService;
        private readonly ProductGenderService _productGenderService;
        private readonly ProductMaterialService _productMaterialService;

        public ProductController(
            ILogger<ProductController> logger,
            ProductServices productService,
            ProductBrandService brandService,
            ProductCategoryServices categoryService,
            ProductGenderService productGenderService,
            ProductMaterialService productMaterialService
            )
        {
            _logger = logger;
            _productService = productService;
            _brandService = brandService;
            _categoryService = categoryService;
            _productGenderService = productGenderService;
            _productMaterialService = productMaterialService;
        }


        [HttpPost("Create")]
        public async Task<ActionResult<ResponseModel<bool>>> Create([FromForm]ProductRequestDTO productModel)
        {
            var responseModel = new ResponseModel<bool>();
            try
            {
                var isProductAdded = await _productService.AddProductAsync(productModel);

                responseModel.Data = isProductAdded;

                return StatusCode(200, responseModel);
            }
            catch (ValidationException ex)
            {
                responseModel.Status = ResponseStatus.Error.ToString();
                responseModel.Error.ValidationErrors = FluentValidationUtility.SetValidationErrors(ex.Errors, _logger);
                responseModel.Error.Message = "Validation Error";
                return StatusCode(422, responseModel);
            }
            catch (ArgumentNullException ex)
            {
                responseModel.Status = ResponseStatus.Error.ToString();
                responseModel.Error.Message = ex.Message;
                return StatusCode(400, responseModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Api/ProductController::Create -> {ex.Message}");
                responseModel.Status = ResponseStatus.Error.ToString();
                responseModel.Error.Message = ex.Message;
                return StatusCode(500, responseModel);
            }
        }
    }
}
