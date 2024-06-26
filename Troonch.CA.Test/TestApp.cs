﻿using Microsoft.Extensions.Logging;
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

                string userLangauge = Thread.CurrentThread.CurrentUICulture.Name;

                //var products = await _productService.GetProductsAsync("nike",2,25);

                //var productsPublished = await _productService.GetProductsPublishedAsync(null);
                var isProductAdded = await _productService.AddProductAsync(new ProductRequestDTO
                {
                    Name = "",
                    IsPublished = true,
                    ProductBrandId = Guid.Parse("08dc60b3-a4dc-4b98-8c50-1144b04ad0a3"),
                    ProductCategoryId = Guid.Parse("08dc6188-f2f9-4b89-8b6e-af0ce1b94b3d"),
                    ProductGenderId = Guid.Parse("d0319a0f-0088-11ef-a87d-00ffe260d4ac"),
                    ProductMaterialId = Guid.Parse("29a06c18-00bb-11ef-ac7b-00ffe260d4ac")
                });


                //var isProductUp = await _productService.UpdateProductAsync(Guid.Parse("08dc62dd-9c3c-44f3-89fc-78cb0827c7d2"), new ProductRequestDTO
                //{
                //    Name = "Prodotto Test 3",
                //    IsPublished = true,
                //    ProductBrandId = Guid.Parse("08dc60b3-a4dc-4b98-8c50-1144b04ad0a3"),
                //    ProductCategoryId = Guid.Parse("08dc6188-f2f9-4b89-8b6e-af0ce1b94b3d"),
                //    ProductGenderId = Guid.Parse("d0319a0f-0088-11ef-a87d-00ffe260d4ac"),
                //    ProductMaterialId = Guid.Parse("29a06c18-00bb-11ef-ac7b-00ffe260d4ac")
                //});

                
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
