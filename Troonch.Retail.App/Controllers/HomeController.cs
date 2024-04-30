using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Troonch.Retail.App.Models;
using Troonch.RetailSales.Product.Application.Services;

namespace Troonch.Retail.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProductServices _productService;

        public HomeController(ILogger<HomeController> logger, ProductServices productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async  Task<IActionResult> Test()
        {
            var products = await _productService.GetProductsAsync(null);
            return View(products);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}