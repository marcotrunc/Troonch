using Microsoft.EntityFrameworkCore;
using Troonch.DataAccess.Base.Repositories;
using Troonch.Sales.DataAccess;

namespace Troonch.RetailSales.Product.DataAccess.Repositories
{
    public sealed class ProductRepository : BaseRepository<SalesEntity.Product, RetailSalesProductDataContext>
    {
        public ProductRepository(RetailSalesProductDataContext dbContext) : base(dbContext)
        {
        }

        public async Task<SalesEntity.Product?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Products
                .SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> IsNameUniqueAync(string name)
        {
            return !await _dbContext.Products.AnyAsync(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
