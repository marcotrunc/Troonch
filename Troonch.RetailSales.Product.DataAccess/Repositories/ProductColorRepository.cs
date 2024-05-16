using Troonch.DataAccess.Base.Repositories;
using Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces;
using Troonch.Sales.DataAccess;

namespace Troonch.RetailSales.Product.DataAccess.Repositories;

public sealed class ProductColorRepository : BaseRepository<SalesEntity.ProductColor, RetailSalesProductDataContext>, IProductColorRepository
{
    public ProductColorRepository(RetailSalesProductDataContext dbContext) : base(dbContext)
    {
    }
}
