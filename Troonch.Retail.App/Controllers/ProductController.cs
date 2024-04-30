using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Troonch.Application.Base.Utilities;
using Troonch.Retail.App.Models;
using Troonch.RetailSales.Product.Application.Services;
using Troonch.RetailSales.Product.Domain.DTOs.Requests;

namespace Troonch.Retail.App.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly ProductServices _productService;
        private readonly ProductBrandService _brandService;
        private readonly ProductCategoryServices _categoryService;
        private readonly ProductGenderService _productGenderService;
        private readonly ProductMaterialService _productMaterialService;

        [ActivatorUtilitiesConstructor]
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

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetProductsAsync(null);
            ViewData["Title"] = "Prodotti";
            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                await GetProductBagForm();
            }
            catch (Exception ex)
            {
                _logger.LogError($"ProductController::Create -> {ex.Message}");
            }

            var productModel = new ProductRequestDTO();

            return View("Create", productModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductRequestDTO model)
        {
            try
            {
                var isProductAdded = await _productService.AddProductAsync(model);

            }
            catch(ValidationException ex) 
            {
                ModelState.SetModelState(ex.Errors, _logger);
                await GetProductBagForm();
                return View("Create", model);
            }
            catch(Exception ex)
            {
                _logger.LogError($"ProductController::Create -> {ex.Message}");
                return View("Error", new ErrorViewModel { RequestId = ex.Message});
            }

            return RedirectToAction("Index");
        }

        private async Task GetProductBagForm()
        {
            var brands = await _brandService.GetAllProductBrandAsync();
            var categories = await _categoryService.GetProductCategoriesAsync();
            var genders = await _productGenderService.GetProductGendersAsync();
            var materials = await _productMaterialService.GetAllProductMaterialAsync();

            ViewBag.Brands = brands;
            ViewBag.Categories = categories;
            ViewBag.Genders = genders;
            ViewBag.Materials = materials;
        }
    }
}
