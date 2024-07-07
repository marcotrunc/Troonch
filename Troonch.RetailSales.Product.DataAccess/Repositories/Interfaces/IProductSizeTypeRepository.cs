using Troonch.DataAccess.Base.Repositories;
using Troonch.Sales.Domain.Entities;

namespace Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces;

public interface IProductSizeTypeRepository : IBaseRepository<ProductSizeType>
{
    Task<bool> Exists(Guid id);
}