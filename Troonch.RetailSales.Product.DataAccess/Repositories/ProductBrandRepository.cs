using Troonch.DataAccess.Base.Repositories;
using Troonch.Sales.DataAccess;
using Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Troonch.RetailSales.Product.DataAccess.Repositories
{
    public sealed class ProductBrandRepository : BaseRepository<SalesEntity.ProductBrand, RetailSalesProductDataContext>, IProductBrandRepository
    {
        public ProductBrandRepository(RetailSalesProductDataContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> IsUniqueNameAsync(string name)
        {
            return !await _dbContext.ProductBrands.AnyAsync(p => p.Name == name);
        }
    }
}
