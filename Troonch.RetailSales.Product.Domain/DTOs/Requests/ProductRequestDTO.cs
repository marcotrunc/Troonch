using System;
using System.Collections.Generic;
using System.Text;

namespace Troonch.RetailSales.Product.Domain.DTOs.Requests
{
    public class ProductRequestDTO
    {
        public Guid? Id { get; set; } = Guid.Empty;
        public string Name { get; set; }
        public string Description { get; set; } = null;
        public bool IsDeleted { get; set; } = false;
        public bool IsPublished { get; set; } = true;
        public string CoverImageLink { get; set; } = null;
        public Guid ProductGenderId { get; set; } 
        public Guid ProductBrandId { get; set; }
        public Guid ProductCategoryId { get; set; }
        public Guid ProductMaterialId { get; set; } = Guid.Empty;
    }
}
