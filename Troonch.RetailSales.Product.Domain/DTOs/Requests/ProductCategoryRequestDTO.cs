using System;

namespace Troonch.RetailSales.Product.Domain.DTOs.Requests
{
    public class ProductCategoryRequestDTO
    {
        public Guid? Id { get; set; } = Guid.Empty;
        public string Name { get; set; }
        public Guid ProductSizeTypeId { get; set; }
    }
}
