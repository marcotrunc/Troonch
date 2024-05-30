using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Troonch.Domain.Base.DTOs.Response;
using Troonch.RetailSales.Product.Application.Services;
using Troonch.RetailSales.Product.Domain.DTOs.Requests;

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

        // GET: Brands/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Brands/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Brands/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Brands/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Brands/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Brands/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
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
