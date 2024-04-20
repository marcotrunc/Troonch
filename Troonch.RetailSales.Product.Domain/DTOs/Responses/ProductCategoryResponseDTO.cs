using System;
using System.Collections.Generic;
using System.Text;

namespace Troonch.RetailSales.Product.Domain.DTOs.Responses
{
    public class ProductCategoryResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ProductSizeTypeId { get; set; }
        public string ProductSizeTypeName { get; set; }
    }
}
