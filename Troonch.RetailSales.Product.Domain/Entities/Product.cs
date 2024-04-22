using System;
using System.Collections.Generic;
using Troonch.Domain.Base.Entities;

namespace Troonch.Sales.Domain.Entities
{
    public class Product : ProductBaseEntity
    {
        public Guid ProductGenderId { get; set; }
        public Guid ProductBrandId { get; set; }
        public Guid ProductCategoryId { get; set; }
        public Guid? ProductMaterialId { get; set; } = null;

        #region Navigation Properties
            public ICollection<ProductItem> ProductItems { get; set; } = new List<ProductItem>();
            public ProductGender ProductGender { get; set; }
            public ProductBrand ProductBrand { get; set; }
            public ProductCategory ProductCategory { get; set; }
            public ProductMaterial ProductMaterial { get; set; }
            public List<ProductTagLookup> ProductTags { get; } = new List<ProductTagLookup>();
        #endregion
    }
}
