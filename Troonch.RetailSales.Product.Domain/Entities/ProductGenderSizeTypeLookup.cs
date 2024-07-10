using System;

namespace Troonch.Sales.Domain.Entities
{
    public class ProductGenderSizeTypeLookup 
    {
        public Guid ProductGenderId { get; set; }
        public Guid ProductSizeTypeId { get; set; }

        #region Navigation Properties
        public ProductGender ProductGender { get; set; }
        public ProductSizeType ProductSizeType { get; set; }
        #endregion
    }
}
