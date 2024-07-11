using System;
using System.Collections.Generic;
using Troonch.Domain.Base.Entities;
using Troonch.Sales.Domain.Entities;

namespace Troonch.RetailSales.Product.Domain.DTOs.Responses
{
    public class ProductCategoryResponseDTO : BaseEntity
    {
        public string Name { get; set; }
        public Guid ProductSizeTypeId { get; set; }
        public string ProductSizeTypeName { get; set; }
        public List<ProductGender> productGenders { get; set; }
    }
}
