using System;
using System.Collections.Generic;
using System.Text;
using Troonch.Domain.Base.Entities;
using Troonch.Domain.Base.Enums;

namespace Troonch.Sales.Domain.Entities
{
    public class ProductItem : BaseEntity
    {
        public Guid ProductId { get; set; }
        public Guid ProductSizeOptionId { get; set; }
        public Guid ProductColorId { get; set; }
        public CurrencyBase Currency { get; set; } = CurrencyBase.EUR;
        public decimal OriginalPrice { get; set; }
        public decimal SalePrice { get; set; }
        public string Barcode { get; set; }
        public int QuantityAvailable { get; set; }


        #region Navigation Properties
            public Product Product { get; set; }
            public ProductSizeOption ProductSizeOption { get; set; }
            public ProductColor ProductColor { get; set; }
        #endregion
    }
}
