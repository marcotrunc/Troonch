using Troonch.DataAccess.Base.Repositories;
using Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces;
using Troonch.Sales.DataAccess;

namespace Troonch.RetailSales.Product.DataAccess.Repositories;

public sealed class ProductMaterialRepository : BaseRepository<SalesEntity.ProductMaterial, RetailSalesProductDataContext>, IProductMaterialRepository
{
    public ProductMaterialRepository(RetailSalesProductDataContext dbContext) : base(dbContext)
    {
    }
}
