using Troonch.DataAccess.Base.Repositories;
using Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces;
using Troonch.Sales.DataAccess;
using Microsoft.EntityFrameworkCore;
using Troonch.Sales.Domain.Entities;

namespace Troonch.RetailSales.Product.DataAccess.Repositories;

public sealed class ProductCategoryRepository : BaseRepository<ProductCategory, RetailSalesProductDataContext>, IProductCategoryRepository
{
    public ProductCategoryRepository(RetailSalesProductDataContext dbContext) : base(dbContext)
    {
    }
    public async Task<bool> IsUniqueNameAsync(Guid? id, string name)
    {
        if (String.IsNullOrWhiteSpace(name))
        {
            return true;
        }

        if (id == null || id == Guid.Empty)
        {
            return !await _dbContext.ProductCategories.AnyAsync(p => p.Name == name.Trim());
        }

        return !await _dbContext.ProductCategories.Where(p => p.Id != id).AnyAsync(p => p.Name == name.Trim());
    }

    public async Task<IEnumerable<ProductCategory>> GetAllProductCategoriesWithSizeAsync()
    {
        return await _dbContext.ProductCategories
            .AsNoTracking()
            .Include(pc => pc.ProductSizeType)
            .ToListAsync();
    }

    public async Task<ProductCategory?> GetCategoryWithSizeAsync(Guid id)
    {
        return await _dbContext.ProductCategories
             .AsNoTracking()
             .Include(pc => pc.ProductSizeType)
             .FirstOrDefaultAsync(pc => pc.Id == id);
    }
}
