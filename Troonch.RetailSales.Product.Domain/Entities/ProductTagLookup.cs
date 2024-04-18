using System;
using System.Collections.Generic;
using System.Text;

namespace Troonch.Sales.Domain.Entities
{
    public class ProductTagLookup 
    {
        public Guid ProductId { get; set; }
        public Guid TagId { get; set; }

        #region Navigation Properties
        public Product Product { get; set; }
        public ProductTag ProductTag { get; set; }
        #endregion
    }
}
