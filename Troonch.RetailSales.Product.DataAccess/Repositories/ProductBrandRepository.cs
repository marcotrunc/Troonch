using SalesEntity = Troonch.Sales.Domain.Entities;
using Troonch.DataAccess.Base.Repositories;
using Troonch.Sales.DataAccess;
using Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces;

namespace Troonch.RetailSales.Product.DataAccess.Repositories
{
    public sealed class ProductBrandRepository : BaseRepository<SalesEntity.ProductBrand, RetailSalesProductDataContext>, IProductBrandRepository
    {
        public ProductBrandRepository(RetailSalesProductDataContext dbContext) : base(dbContext)
        {
        }

    }
}
