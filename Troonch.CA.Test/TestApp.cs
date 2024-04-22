using Microsoft.Extensions.Logging;
using Troonch.RetailSales.Product.Application.Services;
using Troonch.RetailSales.Product.Domain.DTOs.Requests;

namespace Troonch.CA.Test
{
    public class TestApp
    {
        private readonly ILogger<TestApp> _logger;
        private readonly ProductServices _productService;
        public TestApp(ILogger<TestApp> logger, ProductServices productService) 
        { 
           _logger = logger;
           _productService = productService;    
        }

        public async Task Run(string[] args)
        {
            try
            {
                _logger.LogInformation($"Start Test at {DateTime.UtcNow}");

                var isProductAdded = await _productService.AddProductAsync(new ProductRequestDTO
                {
                    Name = "Prodotto Test 1",
                    IsPublished = true,
                    ProductBrandId = Guid.Parse("08dc60b3-a4dc-4b98-8c50-1144b04ad0a3"),
                    ProductCategoryId = Guid.Parse("08dc6188-f2f9-4b89-8b6e-af0ce1b94b3d"),
                    ProductGenderId = Guid.Parse("d0319a0f-0088-11ef-a87d-00ffe260d4ac"),
                    
                });

                _logger.LogInformation($"ProductAdded -> {isProductAdded}");
                _logger.LogInformation($"Finish Test at {DateTime.UtcNow}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            Console.ReadLine();
        }
    }
}
