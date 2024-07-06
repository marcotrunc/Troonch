using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Troonch.Application.Base.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Troonch.RetailSales.Product.Application.Services;
using Troonch.RetailSales.Product.Domain.DTOs.Requests;

namespace Troonch.RetailSales.Product.Presentation.Controllers;

[Authorize]
public class CategoriesController : Controller
{
    private readonly ILogger<CategoriesController> _logger;
    private readonly ProductCategoryServices _productCategoryServices;
    public CategoriesController(
            ILogger<CategoriesController> logger, 
            ProductCategoryServices productCategoryServices
        )
    {
        _logger = logger;
        _productCategoryServices = productCategoryServices;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var categories = await _productCategoryServices.GetProductCategoriesAsync();

            if (categories is null) 
            { 
                throw new ArgumentNullException(nameof(categories));
            }

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
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        try
        {
            var categoryModel = new ProductCategoryRequestDTO();

            return View(categoryModel);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError($"CategoriesController::Create GET -> {ex.Message}");
            throw ex;
        }
        catch (Exception ex)
        {
            _logger.LogError($"CategoriesController::Create GET -> {ex.Message}");
            throw ex;
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductCategoryRequestDTO categoryModel)
    {
        try
        {
            var isCategoryAdded = await _productCategoryServices.AddProductCategoryAsync(categoryModel);

            if (!isCategoryAdded)
            {
                throw new Exception(nameof(isCategoryAdded));
            }
            
            TempData["succeeded"] = isCategoryAdded;
            TempData["message"] = $"La categoria {categoryModel.Name} è stata creata !";

            return RedirectToAction("Index");
        }
        catch (ValidationException ex)
        {
            ModelState.SetModelState(ex.Errors, _logger);

            return View(categoryModel);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError($"CategoriesController::Create POST -> {ex.Message}");
            throw ex;
        }
        catch (Exception ex)
        {
            _logger.LogError($"CategoriesController::Create POST -> {ex.Message}");
            throw ex;
        }
    }
    [HttpGet("Categories/{id}/Update")]
    public async Task<IActionResult> Update(string id)
    {
        try
        {
            var isParsed = Guid.TryParse(id, out var categoryId);

            if (!isParsed)
            {
                _logger.LogError($"CategoriesController::Update GET -> id - {id} not parsed");
            }

            var categoryModel = await _productCategoryServices.BuildProductCategoryToUpdateAsync(categoryId);

            return View(categoryModel);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError($"CategoriesController::Create GET -> {ex.Message}");
            throw ex;
        }
        catch (Exception ex)
        {
            _logger.LogError($"CategoriesController::Create GET -> {ex.Message}");
            throw ex;
        }
    }
    [HttpPost("Categories/{id}/Update")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(string id, ProductCategoryRequestDTO categoryModel)
    {
        try
        {
            var isParsed = Guid.TryParse(id, out var categoryId);

            if (!isParsed)
            {
                _logger.LogError($"CategoriesController::Update POST -> id - {id} not parsed");
            }
            var isCategoryUpdated = await _productCategoryServices.UpdateProductCategoryAsync(categoryId,categoryModel);

            if (!isCategoryUpdated)
            {
                throw new Exception(nameof(isCategoryUpdated));
            }

            TempData["succeeded"] = isCategoryUpdated;
            TempData["message"] = $"La categoria {categoryModel.Name} è stata aggiornata";

            return RedirectToAction("Index");
        }
        catch (ValidationException ex)
        {
            ModelState.SetModelState(ex.Errors, _logger);

            return View(categoryModel);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError($"CategoriesController::Create POST -> {ex.Message}");
            throw ex;
        }
        catch (Exception ex)
        {
            _logger.LogError($"CategoriesController::Create POST -> {ex.Message}");
            throw ex;
        }
    }

    [HttpDelete("Categories/{id?}/Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var isParsed = Guid.TryParse(id, out var categoryId);

            if (!isParsed)
            {
                _logger.LogError($"CategoriesController::Delete id - {id} not parsed");
            }
            
            var isCategoryDeleted = await _productCategoryServices.RemoveProductCategoryAsync(categoryId);

            if (!isCategoryDeleted)
            {
                throw new Exception(nameof(isCategoryDeleted));
            }

            TempData["succeeded"] = isCategoryDeleted;
            TempData["message"] = $"La categoria  è stata eliminata!";

            return RedirectToAction("Index");
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError($"CategoriesController::Create POST -> {ex.Message}");
            throw ex;
        }
        catch (Exception ex)
        {
            _logger.LogError($"CategoriesController::Create POST -> {ex.Message}");
            throw ex;
        }
    }
}

