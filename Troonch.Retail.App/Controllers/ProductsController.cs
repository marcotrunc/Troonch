﻿using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Troonch.Application.Base.Utilities;
using Troonch.Domain.Base.DTOs.Response;
using Troonch.Retail.App.Models;
using Troonch.RetailSales.Product.Application.Services;
using Troonch.RetailSales.Product.Domain.DTOs.Requests;
using Troonch.RetailSales.Product.Domain.DTOs.Responses;
using Troonch.Sales.Domain.Entities;

namespace Troonch.Retail.App.Controllers
{

    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly ProductServices _productService;
        private readonly ProductBrandService _brandService;
        private readonly ProductCategoryServices _categoryService;
        private readonly ProductGenderService _productGenderService;
        private readonly ProductMaterialService _productMaterialService;
        private readonly ProductGenderCategoryService _productGenderCategoryService;

        [ActivatorUtilitiesConstructor]
        public ProductsController(
                                    ILogger<ProductsController> logger,
                                    ProductServices productService,
                                    ProductBrandService brandService,
                                    ProductCategoryServices categoryService,
                                    ProductGenderService productGenderService,
                                    ProductMaterialService productMaterialService,
                                    ProductGenderCategoryService productGenderCategoryService)
        {
            _logger = logger;
            _productService = productService;
            _brandService = brandService;
            _categoryService = categoryService;
            _productGenderService = productGenderService;
            _productMaterialService = productMaterialService;
            _productGenderCategoryService = productGenderCategoryService;
        }


        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] string? searchTerm, [FromQuery] int page = 1, [FromQuery] int pagesize = 10)
        {
            try
            {
                var products = await _productService.GetProductsAsync(searchTerm);
                ViewData["Title"] = "Prodotti";
                return View(products);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError($"ProductsController::Index -> {ex.Message}");
                var responseModel = new ResponseModel<bool>();
                responseModel.Status = ResponseStatus.Error.ToString();
                responseModel.Error.Message = ex.Message;
                return StatusCode(400, responseModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ProductsController::Index -> {ex.Message}");
                var responseModel = new ResponseModel<bool>();
                responseModel.Status = ResponseStatus.Error.ToString();
                responseModel.Error.Message = ex.Message;
                return StatusCode(500, responseModel);
            }
        }

        [HttpGet("Products/Detail/{slug}")]
        public async Task<IActionResult> Detail(string slug)
        {
            try
            {
                var product = await _productService.GetProductBySlugAsync(slug);

                if(product is null)
                {
                    throw new ArgumentNullException(nameof(product));
                }

                return View(product);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError($"ProductsController::Detail -> {ex.Message}");
                var responseModel = new ResponseModel<bool>();
                responseModel.Status = ResponseStatus.Error.ToString();
                responseModel.Error.Message = ex.Message;
                return StatusCode(400, responseModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ProductsController::Detail -> {ex.Message}");
                var responseModel = new ResponseModel<bool>();
                responseModel.Status = ResponseStatus.Error.ToString();
                responseModel.Error.Message = ex.Message;
                return StatusCode(500, responseModel);
            }
        }

        [HttpGet("GetProductForm/{id?}")]
        public async Task<IActionResult> GetProductForm(string? id)
        {
            var productModel = new ProductRequestDTO();
            try
            {
                await GetProductBagForm();

                if (!String.IsNullOrEmpty(id))
                {
                    productModel = await _productService.GetProductByIdForUpdateAsync(Guid.Parse(id));
                    await InserCategorySelectedInViewBag(productModel.ProductCategoryId);
                }

                return PartialView("_Form", productModel);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError($"ProductsController::GetProductForm -> {ex.Message}");
                var responseModel = new ResponseModel<bool>();
                responseModel.Status = ResponseStatus.Error.ToString();
                responseModel.Error.Message = ex.Message;
                return StatusCode(400, responseModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ProductsController::GetProductForm -> {ex.Message}");
                var responseModel = new ResponseModel<bool>();
                responseModel.Status = ResponseStatus.Error.ToString();
                responseModel.Error.Message = ex.Message;
                return StatusCode(500, responseModel);
            }

        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
            
                var productModel = new ProductRequestDTO();
                await GetProductBagForm();

                return PartialView("_Form", productModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ProductsController::Create -> {ex.Message}");
                var responseModel = new ResponseModel<bool>();
                responseModel.Status = ResponseStatus.Error.ToString();
                responseModel.Error.Message = ex.Message;
                return StatusCode(500, responseModel);
            }

        }

        [HttpPost]
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
                _logger.LogError($"Api/ProductController::Create -> {ex.Message}");
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

        

        [HttpPut]
        public async Task<ActionResult<ResponseModel<bool>>> Update([FromBody] ProductRequestDTO productModel)
        {
            var responseModel = new ResponseModel<bool>();

            try
            {

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
                _logger.LogError($"ProductController::Update -> {ex.Message}");
                responseModel.Status = ResponseStatus.Error.ToString();
                responseModel.Error.Message = "Bad Request";
                return StatusCode(400, responseModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ProductController::Update -> {ex.Message}");
                responseModel.Status = ResponseStatus.Error.ToString();
                responseModel.Error.Message = "Internal Server Error";
                return StatusCode(500, responseModel);
            }
        }

        private async Task GetProductBagForm()
        {
            var brands = await _brandService.GetAllProductBrandAsync(null,1,10000);
            ViewBag.Brands = brands.Collections.OrderBy(b => b.Name);

            var categories = await _categoryService.GetProductCategoriesAsync(null,1, 10000);
            ViewBag.Categories = categories.Collections.OrderBy(c => c.Name);

            ViewBag.Genders = await _productGenderService.GetProductGendersAsync();


            var materials = await _productMaterialService.GetAllProductMaterialAsync();
            var materialsListed = materials.ToList();
            materialsListed.Add(new ProductMaterial { Id = Guid.Empty, Value = "Scegli Materiale..." });
            ViewBag.Materials = materialsListed;


            var genderCategories = await _productGenderCategoryService.GetProductGenderCategoriesAsync();
            ViewBag.GenderCategories = genderCategories.Select(gc => new { ProductGenderId = gc.ProductGenderId, ProductCategoryId = gc.ProductCategoryId });
        }
        
        private async Task InserCategorySelectedInViewBag(Guid categoryId)
        {
            var categoriesViewBag = new List<ProductCategoryResponseDTO>();
            var category = await _categoryService.GetProductCategoryByIdAsync(categoryId);

            if (category != null) 
            { 
                categoriesViewBag.Add(category);
            }

            ViewBag.Categories = categoriesViewBag;
        }
    }
}
