using Microsoft.Extensions.Logging;
using Troonch.RetailSales.Product.Application.Services;
using Troonch.RetailSales.Product.Domain.DTOs.Requests;

namespace Troonch.CA.Test
{
    public class TestApp
    {
        private readonly ILogger<TestApp> _logger;
        private readonly ProductCategoryServices _productCategoryService;
        public TestApp(ILogger<TestApp> logger, ProductCategoryServices productCategoryService) 
        { 
           _logger = logger;
           _productCategoryService = productCategoryService;    
        }

        public async Task Run(string[] args)
        {
            try
            {
                _logger.LogInformation($"Start Test at {DateTime.UtcNow}");


                var catUp = await _productCategoryService.UpdateProductCategoryAsync(Guid.Parse("08dc6185-8a6d-4593-8f8d-866e8ccd7f78"), new ProductCategoryRequestDTO
                {
                    Id = Guid.Parse("08dc6185-8a6d-4593-8f8d-866e8ccd7f78"),
                    Name = "Vestiti Bimba",
                    ProductSizeTypeId = Guid.Parse("d637d939-783a-4896-8ae2-be8fe2d20e53"),
                });

                //var catAdd = await _productCategoryService.AddProductCategoryAsync(new ProductCategoryRequestDTO
                //{
                //    Name = "Vestiti Bimbo",
                //    ProductSizeTypeId = Guid.Parse("d637d939-783a-4896-8ae2-be8fe2d20e53"),
                //});


                var cat = await _productCategoryService.GetProductCategoriesAsync();
                Guid id = cat.Where(c => c.Name.Contains("Bimba")).First().Id;
                var rem = await _productCategoryService.RemoveProductCategoryAsync(id);

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
