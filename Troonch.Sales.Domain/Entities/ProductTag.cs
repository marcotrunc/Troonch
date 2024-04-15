using System;
using System.Collections.Generic;
using System.Text;
using Troonch.Domain.Base.Entities;

namespace Troonch.Sales.Domain.Entities
{
    public class ProductTag : BaseEntity
    {
        public string Value{ get; set; }

        #region Navigation Properties
        public List<ProductTagLookup> ProductTags { get; } = new List<ProductTagLookup>();
        #endregion
    }
}
