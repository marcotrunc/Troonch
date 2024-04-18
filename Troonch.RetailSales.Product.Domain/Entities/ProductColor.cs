using System;
using System.Collections.Generic;
using System.Text;
using Troonch.Domain.Base.Entities;

namespace Troonch.Sales.Domain.Entities
{
    public class ProductColor : BaseEntity
    {
        public string Name { get; set; }
        public string HexadecimalValue { get; set; }

        #region Navigation Properties
            public ICollection<ProductItem> ProductItems { get; set; } = new List<ProductItem>();
        #endregion
    }
}
