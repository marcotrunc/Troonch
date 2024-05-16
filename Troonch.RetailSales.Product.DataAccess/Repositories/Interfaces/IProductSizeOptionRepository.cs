using Troonch.DataAccess.Base.Repositories;
using Troonch.Sales.Domain.Entities;

namespace Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces;

public interface IProductSizeOptionRepository : IBaseRepository<ProductSizeOption>
{
    Task<IEnumerable<ProductSizeOption>> GetProductSizeOptionsByTypeIdAsync(Guid typeId);
}
