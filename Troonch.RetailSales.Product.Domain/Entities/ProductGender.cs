using System.Collections.Generic;
using Troonch.Domain.Base.Entities;

namespace Troonch.Sales.Domain.Entities
{
    public class ProductGender : BaseEntity
    {
        public string Name { get; set; }
        #region Navigation Properties
        public ICollection<Product> Products { get; } = new List<Product>();
        public List<ProductGenderCategoryLookup> ProductCategories { get; } = new List<ProductGenderCategoryLookup>();
        #endregion
    }
}
