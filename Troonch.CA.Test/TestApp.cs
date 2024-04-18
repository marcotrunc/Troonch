using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troonch.RetailSales.Product.DataAccess.Repositories;
using Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces;
using Troonch.Sales.Domain.Entities;

namespace Troonch.CA.Test
{
    public class TestApp
    {
        private readonly IProductBrandRepository _brandRepository;
        public TestApp(IProductBrandRepository brandRepository) 
        { 
            _brandRepository = brandRepository;
        }

        public async Task Run(string[] args)
        {
            var newbrand = new ProductBrand
            {
                Name = "Brand di esempio"
            };
            await _brandRepository.AddAsync(newbrand);

            Console.WriteLine("TEST");
        }
    }
}
