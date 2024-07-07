using System;
using Troonch.Domain.Base.Enums;

namespace Troonch.RetailSales.Product.Domain.DTOs.Responses
{
    public class ProductItemResponse
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; } 
        public string ColorName { get; set; } 
        public int QuantityAvailable { get; set; }
        public string Size {  get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal SalePrice { get; set; }
        public CurrencyBase Currency { get; set; }
        public string Barcode { get; set; }
    }
}
