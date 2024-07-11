using System;
using System.Collections.Generic;
using Troonch.Sales.Domain.Entities;

namespace Troonch.RetailSales.Product.Domain.DTOs.Requests
{
    public class ProductCategoryRequestDTO
    {
        public Guid? Id { get; set; } = Guid.Empty;
        public string Name { get; set; }
        public Guid ProductSizeTypeId { get; set; }
        public List<Guid> Genders { get; set; }
    }
}
