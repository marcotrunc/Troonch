using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using Troonch.Application.Base.Utilities;
using Troonch.Domain.Base.DTOs.Response;
using Troonch.RetailSales.Product.Application.Services;
using Troonch.RetailSales.Product.Domain.DTOs.Requests;

namespace Troonch.Retail.App.Controllers
{
    [Authorize]
    public class ItemsController : Controller
    {
        private readonly ILogger<ItemsController> _logger;
        private readonly ProductItemService _productItemService;
        private readonly ProductCategoryServices _productCategoryServices;
        private readonly ProductSizeOptionService _productSizeOptionService;
        public ItemsController(
            ILogger<ItemsController> logger,
            ProductItemService productItemService,
            ProductCategoryServices productCategoryServices,
            ProductSizeOptionService productSizeOptionService
            )
        {
            _logger = logger;
            _productItemService = productItemService;
            _productCategoryServices = productCategoryServices;
            _productSizeOptionService = productSizeOptionService;
        }


        [HttpGet("items/Index/{productId?}")]
        public async Task<IActionResult> Index(string? productId)
        {
            try
            {
                if(productId is null)
                {
                    throw new ArgumentNullException(productId);
                }

                var productItems = await _productItemService.GetProductItemsByProductIdAsync(Guid.Parse(productId));
                
                if(productItems is null)
                {
                    throw new ArgumentNullException($"product items list is null");
                }

                return PartialView("_Index", productItems);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError($"ItemsController::RenderProductItemListByProductId -> {ex.Message}");
                var responseModel = new ResponseModel<bool>();
                responseModel.Status = ResponseStatus.Error.ToString();
                responseModel.Error.Message = ex.Message;
                return StatusCode(400, responseModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ItemsController::RenderProductItemListByProductId -> {ex.Message}");
                var responseModel = new ResponseModel<bool>();
                responseModel.Status = ResponseStatus.Error.ToString();
                responseModel.Error.Message = "Internal Server Error";
                return StatusCode(500, responseModel);
            }
        }

        [HttpGet("GetProductItemsForm/{categoryId}/{productId}/{itemId?}")]
        public async Task<IActionResult> GetProductItemsForm(string categoryId,string productId, string? itemId)
        {
            var itemModel = new ProductItemRequestDTO();

            // Aggiungere il controllo sul productId
            itemModel.ProductId = Guid.Parse(productId);

            try
            {
                if (Guid.TryParse(categoryId,out Guid categoryIdParsed))
                {
                    await GetProductItemsBag(categoryIdParsed);
                }

                if (itemId is not null && Guid.TryParse(itemId, out Guid itemIdParsed))
                {
                    itemModel = await _productItemService.GetProductByIdForUpdateAsync(itemIdParsed);
                }


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
                responseModel.Error.Message = "Internal Server Error";
                return StatusCode(500, responseModel);
            }
        }
        [HttpGet]
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

                return StatusCode(200, responseModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ProductItemsController::GenerateBarcode -> {ex.Message}");
                responseModel.Status = ResponseStatus.Error.ToString();
                responseModel.Error.Message = "Internal Server Error";
                return StatusCode(500, responseModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] ProductItemRequestDTO productItemModel)
        {
            var responseModel = new ResponseModel<bool>();

            try
            {
                var isProductAdded = await _productItemService.AddProductItemAsync(productItemModel);
                responseModel.Data = isProductAdded;

                if (!isProductAdded)
                {
                    throw new Exception();
                }


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
                _logger.LogError($"ProductItemsController::Create -> {ex.Message}");
                responseModel.Status = ResponseStatus.Error.ToString();
                responseModel.Error.Message = ex.Message;
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

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ProductItemRequestDTO productItemModel)
        {
            var responseModel = new ResponseModel<bool>();

            try
            {

                var isProductUpdated = await _productItemService.UpdateProductItemAsync(productItemModel.Id ?? Guid.Empty, productItemModel);

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
                _logger.LogError($"ProductItemsController::Update -> {ex.Message}");
                responseModel.Status = ResponseStatus.Error.ToString();
                responseModel.Error.Message = "Bad Request";
                return StatusCode(400, responseModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ProductItemsController::Update -> {ex.Message}");
                responseModel.Status = ResponseStatus.Error.ToString();
                responseModel.Error.Message = "Internal Server Error";
                return StatusCode(500, responseModel);
            }
        }

        [HttpGet("items/Delete/{productItemId?}")]
        public async Task<IActionResult> Delete(string? productItemId)
        {
            var responseModel = new ResponseModel<bool>();
            try
            {
                if (Guid.TryParse(productItemId, out Guid productItemIdParsed))
                {
                    responseModel.Data = await _productItemService.DeleteProductItemByIdAsync(productItemIdParsed);
                    return StatusCode(200, responseModel);
                }
                else
                {
                    throw new Exception(nameof(productItemId));
                }
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError($"ProductItemsController::Create -> {ex.Message}");
                responseModel.Status = ResponseStatus.Error.ToString();
                responseModel.Error.Message = ex.Message;
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
        private async Task GetProductItemsBag(Guid categoryId)
        {
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
