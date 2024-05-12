using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Troonch.Application.Base.Utilities;
using Troonch.Domain.Base.DTOs.Response;
using Troonch.RetailSales.Product.Application.Services;
using Troonch.RetailSales.Product.Domain.DTOs.Requests;

namespace Troonch.Retail.App.Areas.Api.Controllers
{
    [ApiController]
    [Area("Api")]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly ProductServices _productService;
        private readonly ProductBrandService _brandService;
        private readonly ProductCategoryServices _categoryService;
        private readonly ProductGenderService _productGenderService;
        private readonly ProductMaterialService _productMaterialService;

        public ProductsController(
            ILogger<ProductsController> logger,
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
        public async Task<ActionResult<ResponseModel<bool>>> Create([FromBody] ProductRequestDTO productModel)
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

        [HttpPut("Update")]
        public async Task<ActionResult<ResponseModel<bool>>> Update([FromBody] ProductRequestDTO productModel)
        {
            var responseModel = new ResponseModel<bool>();
            
            try { 

                var isProductUpdated = await _productService.UpdateProductAsync(productModel.Id ?? Guid.Empty, productModel);

                responseModel.Data = isProductUpdated;

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
