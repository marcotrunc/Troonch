using System.Collections.Generic;
using Troonch.Domain.Base.Entities;

namespace Troonch.Sales.Domain.Entities
{
    public class ProductBrand : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        #region Navigation Properties
        public ICollection<Product> Products { get; set; } = new List<Product>();
        #endregion
    }
}
