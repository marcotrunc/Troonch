using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Troonch.Application.Base.Utilities;
using Troonch.Domain.Base.DTOs.Response;
using Troonch.RetailSales.Product.Application.Services;
using Troonch.RetailSales.Product.Domain.DTOs.Requests;

namespace Troonch.RetailSales.Product.Presentation.Controllers;

[Authorize]
public class CategoriesController : Controller
{
    private readonly ILogger<CategoriesController> _logger;
    private readonly ProductCategoryServices _productCategoryServices;
    private readonly ProductSizeTypeService _productSizeTypesService;
    public CategoriesController(
            ILogger<CategoriesController> logger,
            ProductCategoryServices productCategoryServices,
            ProductSizeTypeService productSizeTypesService)
    {
        _logger = logger;
        _productCategoryServices = productCategoryServices;
        _productSizeTypesService = productSizeTypesService;
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] string? searchTerm, [FromQuery] int page = 1, [FromQuery] int pagesize = 10)
    {
        try
        {
            var categories = await _productCategoryServices.GetProductCategoriesAsync(searchTerm, page, pagesize);

            if (categories is null) 
            { 
                throw new ArgumentNullException(nameof(categories));
            }

            ViewData["Title"] = "Categorie";

            return View("Index", categories);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError($"CategoriesController::Index GET -> {ex.Message}");
            throw ex;
        }
        catch (Exception ex)
        {
            _logger.LogError($"CategoriesController::Index GET -> {ex.Message}");
            throw ex;
        }
    }

    [HttpGet("Categories/Detail/{id}")]
    public async Task<IActionResult> Detail(string id)
    {
        try
        {
            var isParsed = Guid.TryParse(id, out var categoryId);

            if (!isParsed) 
            {
                _logger.LogError($"CategoriesController::Detail id - {id} not parsed");
            }

            var category = await _productCategoryServices.GetProductCategoryByIdAsync(categoryId);

            if(category is null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            return View(category);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError($"CategoriesController::Detail GET -> {ex.Message}");
            throw ex;
        }
        catch (Exception ex)
        {
            _logger.LogError($"CategoriesController::Detail GET -> {ex.Message}");
            throw ex;
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductCategoryRequestDTO categoryModel)
    {
        var responseModel = new ResponseModel<bool>();

        try
        {
            var isCategoryAdded = await _productCategoryServices.AddProductCategoryAsync(categoryModel);
            responseModel.Data = isCategoryAdded;

            if (!isCategoryAdded)
            {
                throw new Exception(nameof(isCategoryAdded));
            }


            return StatusCode(200, responseModel);
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
            _logger.LogError($"CategoriesController::Create POST -> {ex.Message}");
            responseModel.Status = ResponseStatus.Error.ToString();
            responseModel.Error.Message = "Bad Request";
            return StatusCode(400, responseModel);
        }
        catch (Exception ex)
        {
            _logger.LogError($"CategoriesController::Create POST -> {ex.Message}");
            _logger.LogError($"ProductItemsController::Create -> {ex.Message}");
            responseModel.Status = ResponseStatus.Error.ToString();
            responseModel.Error.Message = "Internal Server Error";
            return StatusCode(500, responseModel);
        }
    }
   
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ProductCategoryRequestDTO categoryModel)
    {
        var responseModel = new ResponseModel<bool>();
        try
        {
            var isCategoryUpdated = await _productCategoryServices.UpdateProductCategoryAsync(categoryModel.Id ?? Guid.Empty, categoryModel);
            responseModel.Data = isCategoryUpdated;

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
            _logger.LogError($"CategoriesController::Create POST -> {ex.Message}");
            responseModel.Status = ResponseStatus.Error.ToString();
            responseModel.Error.Message = "Bad Request";
            return StatusCode(400, responseModel);
        }
        catch (Exception ex)
        {
            _logger.LogError($"CategoriesController::Create POST -> {ex.Message}");
            responseModel.Status = ResponseStatus.Error.ToString();
            responseModel.Error.Message = "Internal Server Error";
            return StatusCode(500, responseModel);
        }
    }

    [HttpDelete("Categories/{id?}")]
    public async Task<IActionResult> Delete(string? id)
    {
        var responseModel = new ResponseModel<bool>();
        try
        {
            var isParsed = Guid.TryParse(id, out var categoryId);

            if (!isParsed)
            {
                _logger.LogError($"CategoriesController::Delete id - {id} not parsed");
                throw new ArgumentNullException(nameof(id));
            }
            
            var isCategoryDeleted = await _productCategoryServices.RemoveProductCategoryAsync(categoryId);

            if (!isCategoryDeleted)
            {
                throw new Exception(nameof(isCategoryDeleted));
            }

            responseModel.Data = isCategoryDeleted;

            return StatusCode(200, responseModel);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError($"CategoriesController::Create POST -> {ex.Message}");
            responseModel.Status = ResponseStatus.Error.ToString();
            responseModel.Error.Message = ex.Message;
            return StatusCode(400, responseModel);
        }
        catch (Exception ex)
        {
            _logger.LogError($"CategoriesController::Create POST -> {ex.Message}");
            responseModel.Status = ResponseStatus.Error.ToString();
            responseModel.Error.Message = "Internal Server Error";
            return StatusCode(500, responseModel);
        }
    }

    #region Render Partial Views
    [HttpGet("Categories/GetCategoryForm/{categoryId?}")]
    public async Task<IActionResult> GetCategoryForm(string? categoryId)
    {
        var categoryModel = new ProductCategoryRequestDTO();

        try
        {

            await GetProductCategoryBagForm();

            if (!String.IsNullOrWhiteSpace(categoryId) && Guid.TryParse(categoryId, out Guid categoryIdParsed))
            {
                categoryModel = await _productCategoryServices.BuildProductCategoryToUpdateAsync(categoryIdParsed);
            }


            return PartialView("_Form", categoryModel);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError($"CategoriesController::GetCategoryForm GET -> {ex.Message}");
            var responseModel = new ResponseModel<bool>();
            responseModel.Status = ResponseStatus.Error.ToString();
            responseModel.Error.Message = ex.Message;
            return StatusCode(400, responseModel);
        }
        catch (Exception ex)
        {
            var responseModel = new ResponseModel<bool>();
            _logger.LogError($"CategoriesController::GetCategoryForm GET -> {ex.Message}");
            responseModel.Status = ResponseStatus.Error.ToString();
            responseModel.Error.Message = "Internal Server Error";
            return StatusCode(500, responseModel);
        }
    }
    #endregion

    #region private Methods

    private async Task GetProductCategoryBagForm()
    {
        ViewBag.ProductSizeTypes = await _productSizeTypesService.GetAllProductSizeTypesAsync();
    }

    #endregion
}

