using System;
using System.Collections.Generic;
using System.Text;
using Troonch.Domain.Base.Entities;

namespace Troonch.Sales.Domain.Entities
{
    public class ProductMaterial : BaseEntity
    {
        public string Value { get; set; }

        #region Navigation Properties
            public ICollection<Product> Products { get; set; } = new List<Product>();
        #endregion
    }
}
