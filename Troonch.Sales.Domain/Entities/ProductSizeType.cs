using System;
using System.Collections.Generic;
using System.Text;
using Troonch.Domain.Base.Entities;

namespace Troonch.Sales.Domain.Entities
{
    public class ProductSizeType : BaseEntity
    {
        public string Name{ get; set; }
        
        #region Navigation Properties
        public ICollection<ProductSizeOption> ProductSizeOptions { get; set; } = new List<ProductSizeOption>();
        public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
        #endregion
    }
}
