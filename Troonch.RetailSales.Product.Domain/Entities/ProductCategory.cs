using System;
using System.Collections.Generic;
using System.Text;
using Troonch.Domain.Base.Entities;

namespace Troonch.Sales.Domain.Entities
{
    public class ProductCategory : BaseEntity
    {
        public Guid ProductSizeTypeId { get; set; } 
        public string Name { get; set; }

        #region Navigation Properties
        public ProductSizeType ProductSizeType { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public List<ProductGenderCategoryLookup> ProductGenders { get; } = new List<ProductGenderCategoryLookup>();
        #endregion
    }
}
