using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using Troonch.Domain.Base.Entities;

namespace Troonch.Sales.Domain.Entities
{
    public class ProductSizeOption : BaseEntity
    {
        public Guid ProductSizeTypeId { get; set; }
        public string Value { get; set; }
        public int Sort {  get; set; }
        
        #region Navigation Properties
        public ProductSizeType ProductSizeType { get; set; }
        public ICollection<ProductItem> ProductItems { get; set; } = new List<ProductItem>();
        #endregion
    }
}
