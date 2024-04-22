using System;
using System.Collections.Generic;
using System.Text;

namespace Troonch.RetailSales.Product.Domain.DTOs.Responses
{
    public class ProductResponseDTO
    {
        public Guid Id { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; } 
        public bool IsPublished { get; set; }
        public string CoverImageLink { get; set; }
        public Guid ProductGenderId { get; set; }
        public string ProductGenderName { get; set; }
        public Guid ProductBrandId { get; set; }
        public string ProductBrandName { get; set; }
        public Guid ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; }
        public Guid? ProductMaterialId { get; set; }
        public string ProductMaterialName { get; set; } = string.Empty;
    }
}
