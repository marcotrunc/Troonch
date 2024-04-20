using Microsoft.Extensions.Logging;
using Troonch.RetailSales.Product.Application.Services;
using Troonch.RetailSales.Product.Domain.DTOs.Requests;

namespace Troonch.CA.Test
{
    public class TestApp
    {
        private readonly ILogger<TestApp> _logger;
        private readonly ProductBrandService _productBrandService;
        public TestApp(ILogger<TestApp> logger, ProductBrandService productBrandService) 
        { 
           _logger = logger;
            _productBrandService = productBrandService;
        }

        public async Task Run(string[] args)
        {
            try
            {
                _logger.LogInformation($"Start Test at {DateTime.UtcNow}");

                var brands = await _productBrandService.GetAllProductBrandAsync();

                Console.WriteLine("brands", brands);

                //var newBrand = await _productBrandService.AddProductBrandAsync(new ProductBrandRequestDTO
                //{
                //    Name = "Nuovo Brando!!",
                //    Description = "Descrizione del nuovo Brando"
                //});
                //Console.WriteLine("newBrand", newBrand);


                var brandSearched = await _productBrandService.GetProductBrandByAsync(Guid.Parse("08dc6173-76ff-4f76-87db-039bf9659cc4"));
                Console.WriteLine("brandSearched", brandSearched);

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
