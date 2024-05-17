using Microsoft.EntityFrameworkCore;
using Troonch.DataAccess.Base.Repositories;
using Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces;
using Troonch.Sales.DataAccess;
using Troonch.Sales.Domain.Entities;

namespace Troonch.RetailSales.Product.DataAccess.Repositories;

public sealed class ProductItemRepository : BaseRepository<SalesEntity.ProductItem, RetailSalesProductDataContext>, IProductItemRepository
{
    public ProductItemRepository(RetailSalesProductDataContext dbContext) : base(dbContext)
    {}
    public async Task<IEnumerable<ProductItem>> GetProductItemsByProductIdAsync(Guid productId)
    {
        return await _dbContext.ProductItems.AsNoTracking()
                    .Include(pi => pi.ProductColor)
                    .Include(pi => pi.ProductSizeOption)
                    .OrderByDescending(pi => pi.UpdatedOn)
                    .Where(pi => pi.ProductId == productId)
                    .ToListAsync();
    }

    public async Task<bool> IsUniqueBarcodeAsync(Guid? id, string barcode)
    {
        if (id is null || id == Guid.Empty) 
        {
            return !await _dbContext.ProductItems.AsNoTracking().AnyAsync(pi => pi.Barcode == barcode.Trim());  
        }

        return !await _dbContext.ProductItems.AsNoTracking().Where(pi => pi.Id != id).AnyAsync(pi => pi.Barcode == barcode.Trim());
    }
}
