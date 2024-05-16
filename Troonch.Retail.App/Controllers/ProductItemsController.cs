using Microsoft.AspNetCore.Mvc;
using System;
using Troonch.Domain.Base.DTOs.Response;
using Troonch.RetailSales.Product.Application.Services;
using Troonch.RetailSales.Product.Domain.DTOs.Requests;

namespace Troonch.Retail.App.Controllers
{
    public class ProductItemsController : Controller
    {
        private readonly ILogger<ProductItemsController> _logger;
        private readonly ProductColorService _productColorService;
        private readonly ProductCategoryServices _productCategoryServices;
        private readonly ProductSizeOptionService _productSizeOptionService;
        public ProductItemsController(
            ILogger<ProductItemsController> logger,
            ProductColorService productColorService,
            ProductCategoryServices productCategoryServices,
            ProductSizeOptionService productSizeOptionService
            )
        {
            _logger = logger;
            _productColorService = productColorService;
            _productCategoryServices = productCategoryServices;
            _productSizeOptionService = productSizeOptionService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetProductItemsForm/{categoryId}/{productId}/{itemId?}")]
        public async Task<IActionResult> GetProductItemsForm(string categoryId,string productId, string? itemId)
        {
            var itemModel = new ProductItemRequestDTO();

            // Aggiungere il controllo sul productId
            itemModel.ProductId = Guid.Parse(productId);

            try
            {
                await GetProductItemsBag(Guid.Parse(categoryId));
                
                return PartialView("_Form", itemModel);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError($"ProductItemsController::GetProductItemsForm -> {ex.Message}");
                var responseModel = new ResponseModel<bool>();
                responseModel.Status = ResponseStatus.Error.ToString();
                responseModel.Error.Message = ex.Message;
                return StatusCode(400, responseModel);
            }
            catch (Exception ex)
            {
                var responseModel = new ResponseModel<bool>();
                _logger.LogError($"ProductItemsController::GetProductItemsForm -> {ex.Message}");
                responseModel.Status = ResponseStatus.Error.ToString();
                responseModel.Error.Message = ex.Message;
                return StatusCode(500, responseModel);
            }
        }

        [HttpGet("GenerateBarcode")]
        public async Task<IActionResult> GenerateBarcode()
        {
            var responseModel = new ResponseModel<string>();
            try
            {
                var random = new Random();

                int[] barcodeDigits = new int[12];

                for (int i = 0; i < 12; i++)
                {
                    barcodeDigits[i] = random.Next(0, 10);
                }

                int sum = 0;
                for (int i = 0; i < 12; i++)
                {
                    int digit = barcodeDigits[i];
                    if (i % 2 == 0)
                    {
                        sum += digit;
                    }
                    else
                    {
                        sum += digit * 3;
                    }
                }
                int checkDigit = (10 - (sum % 10)) % 10;

                string barcode = string.Join("", barcodeDigits) + checkDigit;
                
                responseModel.Data = barcode;

                //Check if is unique
                //var isUnique = await _product.IsBarcodeUnique(barcode);
                //if(!isUnique)
                //{
                //  GenerateBarcode();
                //}

                return StatusCode(200, responseModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ProductItemsController::GenerateBarcode -> {ex.Message}");
                responseModel.Status = ResponseStatus.Error.ToString();
                responseModel.Error.Message = ex.Message;
                return StatusCode(500, responseModel);
            }
        }
        private async Task GetProductItemsBag(Guid categoryId)
        {
            var colors = await _productColorService.GetProductColorsAsync();

            if (colors is null)
            {
                throw new ArgumentNullException(nameof(colors));
            }

            ViewBag.Colors = colors;
            
            var category = await _productCategoryServices.GetProductCategoryByIdAsync(categoryId);

            if(category is null) 
            { 
                throw new ArgumentNullException(nameof(category));
            }

            var productSizeTypeId = category.ProductSizeTypeId;

            if(productSizeTypeId == Guid.Empty)
            {
                throw new Exception(nameof(productSizeTypeId));
            }

            var productSizeOptions = await _productSizeOptionService.GetProductSizeOptionsByTypeIdAsync(productSizeTypeId);

            ViewBag.SizeOptions = productSizeOptions;
        }
    }
}
