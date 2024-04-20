
using System;

namespace Troonch.RetailSales.Product.Domain.DTOs.Requests
{
    public class ProductBrandRequestDTO
    {
        public Guid? Id {  get; set; } = Guid.Empty;
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
