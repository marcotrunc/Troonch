using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troonch.Application.Base.UnitOfWork;
using Troonch.RetailSales.Product.DataAccess.Repositories;
using Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces;
using Troonch.Sales.Domain.Entities;

namespace Troonch.CA.Test
{
    public class TestApp
    {
        private readonly IProductBrandRepository _brandRepository;
        private readonly IUnitOfWork _unitOfWork;
        public TestApp(IProductBrandRepository brandRepository,  IUnitOfWork unitOfWork) 
        { 
            _brandRepository = brandRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Run(string[] args)
        {
            var newbrand = new ProductBrand
            {
                Name = "Brand di esempio 2"
            };
            
            await _brandRepository.AddAsync(newbrand);
            await _unitOfWork.CommitAsync();

            Console.WriteLine("TEST");
        }
    }
}
