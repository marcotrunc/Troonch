using System;

namespace Troonch.RetailSales.Product.Domain.DTOs.Requests
{
    public class ProductGenderCategoryLookupRequestDTO
    {
        public Guid ProductGenderId { get; set; }
        public Guid ProductCategoryId { get; set; }
    }
}
