
using System;
using Troonch.Domain.Base.Enums;

namespace Troonch.RetailSales.Product.Domain.DTOs.Requests
{
    public class ProductItemRequestDTO
    {
        public Guid? Id { get; set; } = Guid.Empty;
        public Guid ProductId { get; set; }
        public Guid ProductSizeOptionId { get; set; }
        public Guid ProductColorId { get; set; }
        public CurrencyBase Currency { get; set; } = CurrencyBase.EUR;
        public decimal OriginalPrice { get; set; }
        public decimal SalePrice { get; set; }
        public string Barcode { get; set; }
        public int QuantityAvailable { get; set; }
    }
}
