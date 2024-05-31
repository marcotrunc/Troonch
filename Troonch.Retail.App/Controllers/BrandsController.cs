using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Troonch.Application.Base.Utilities;
using Troonch.Domain.Base.DTOs.Response;
using Troonch.RetailSales.Product.Application.Services;
using Troonch.RetailSales.Product.Domain.DTOs.Requests;
using Troonch.Sales.Domain.Entities;

namespace Troonch.Retail.App.Controllers
{
    public class BrandsController : Controller
    {
        private readonly ILogger<BrandsController> _logger;
        private readonly ProductBrandService _brandService;

        [ActivatorUtilitiesConstructor]
        public BrandsController(
            ILogger<BrandsController> logger,
            ProductBrandService brandService
            )
        {
            _logger = logger;
            _brandService = brandService;
        }

        // GET: Brands
        public async Task<IActionResult> Index()
        {
            try
            {
                var brands = await _brandService.GetAllProductBrandAsync();
                ViewData["Title"] = "Brands";
                return View(brands);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError($"BrandsController::Index -> {ex.Message}");
                var responseModel = new ResponseModel<bool>();
                responseModel.Status = ResponseStatus.Error.ToString();
                responseModel.Error.Message = ex.Message;
                return StatusCode(400, responseModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"BrandsController::Index -> {ex.Message}");
                var responseModel = new ResponseModel<bool>();
                responseModel.Status = ResponseStatus.Error.ToString();
                responseModel.Error.Message = ex.Message;
                return StatusCode(500, responseModel);
            }
        }

        // GET: Brands/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

       
        // POST: Brands/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductBrandRequestDTO brandModel)
        {
            var responseModel = new ResponseModel<bool>();
            try
            {
                var isBrandAdded = await _brandService.AddProductBrandAsync(brandModel);

                responseModel.Data = isBrandAdded;

                return StatusCode(200,responseModel);
            }
            catch (ValidationException ex)
            {
                _logger.LogError($"BrandsController::Create -> {ex.Message}");
                responseModel.Status = ResponseStatus.Error.ToString();
                responseModel.Error.ValidationErrors = FluentValidationUtility.SetValidationErrors(ex.Errors, _logger);
                responseModel.Error.Message = "Validation Error";
                return StatusCode(422, responseModel);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError($"BrandsController::Create -> {ex.Message}");
                responseModel.Status = ResponseStatus.Error.ToString();
                responseModel.Error.Message = "Bad Request";
                return StatusCode(400, responseModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"BrandsController::Create -> {ex.Message}");
                responseModel.Status = ResponseStatus.Error.ToString();
                responseModel.Error.Message = ex.Message;
                return StatusCode(500, responseModel);
            }
        }



        // PUT: Brands/Update
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ProductBrandRequestDTO brandModel)
        {
            var responseModel = new ResponseModel<bool>();
            try
            {
                var isBrandUpdated = await _brandService.UpdateProductBrandAsync(brandModel.Id ?? Guid.Empty,brandModel);
                responseModel.Data = isBrandUpdated;

                return StatusCode(200,responseModel);
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
                _logger.LogError($"BrandsController::Update -> {ex.Message}");
                responseModel.Status = ResponseStatus.Error.ToString();
                responseModel.Error.Message = "Bad Request";
                return StatusCode(400, responseModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"BrandsController::Update -> {ex.Message}");
                responseModel.Status = ResponseStatus.Error.ToString();
                responseModel.Error.Message = "Internal Server Error";
                return StatusCode(500, responseModel);
            }
        }


        // GET: Brands/Delete/5
        [HttpGet("Brands/Delete/{brandId?}")]
        public async Task<IActionResult> Delete(string? brandId)
        {
            var responseModel = new ResponseModel<bool>();
            try
            {
                if (Guid.TryParse(brandId, out Guid brandIdParsed))
                {
                    responseModel.Data = await _brandService.RemoveProductBrandAsync(brandIdParsed);
                    return StatusCode(200, responseModel);
                }
                else
                {
                    throw new Exception(nameof(brandId));
                }
            }
            catch (ValidationException ex)
            {
                responseModel.Status = ResponseStatus.Error.ToString();
                responseModel.Error.ValidationErrors = FluentValidationUtility.SetValidationErrors(ex.Errors, _logger);
                responseModel.Error.Message = ex.Message;
                return StatusCode(422, responseModel);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError($"ProductItemsController::Create -> {ex.Message}");
                responseModel.Status = ResponseStatus.Error.ToString();
                responseModel.Error.Message = "Bad Request";
                return StatusCode(400, responseModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ProductItemsController::Create -> {ex.Message}");
                responseModel.Status = ResponseStatus.Error.ToString();
                responseModel.Error.Message = "Internal Server Error";
                return StatusCode(500, responseModel);
            }
        }

        #region Render Partial Views
        [HttpGet("GetBrandForm/{brandId?}")]
        public async Task<IActionResult> GetBrandForm(string? brandId)
        {
            var brandModel = new ProductBrandRequestDTO();

            try
            {

                if (brandId is not null && Guid.TryParse(brandId, out Guid brandIdParsed))
                {
                    brandModel = await _brandService.GetProductBrandForUpdateByAsync(brandIdParsed);
                }


                return PartialView("_Form", brandModel);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError($"BrandsController::GetBrandForm -> {ex.Message}");
                var responseModel = new ResponseModel<bool>();
                responseModel.Status = ResponseStatus.Error.ToString();
                responseModel.Error.Message = ex.Message;
                return StatusCode(400, responseModel);
            }
            catch (Exception ex)
            {
                var responseModel = new ResponseModel<bool>();
                _logger.LogError($"BrandsController::GetBrandForm -> {ex.Message}");
                responseModel.Status = ResponseStatus.Error.ToString();
                responseModel.Error.Message = "Internal Server Error";
                return StatusCode(500, responseModel);
            }
        }

        #endregion
    }
}
