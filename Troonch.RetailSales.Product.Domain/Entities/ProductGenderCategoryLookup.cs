using System;

namespace Troonch.Sales.Domain.Entities
{
    public class ProductGenderCategoryLookup 
    {
        public Guid ProductGenderId { get; set; }
        public Guid ProductCategoryId { get; set; }

        #region Navigation Properties
        public ProductGender ProductGender { get; set; }
        public ProductCategory ProductCategory { get; set; }
        #endregion
    }
}
