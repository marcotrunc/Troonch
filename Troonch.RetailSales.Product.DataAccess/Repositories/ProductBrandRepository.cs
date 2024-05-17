using Troonch.DataAccess.Base.Repositories;
using Troonch.Sales.DataAccess;
using Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Troonch.RetailSales.Product.DataAccess.Repositories;

public sealed class ProductBrandRepository : BaseRepository<SalesEntity.ProductBrand, RetailSalesProductDataContext>, IProductBrandRepository
{
    public ProductBrandRepository(RetailSalesProductDataContext dbContext) : base(dbContext)
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
            return !await _dbContext.ProductBrands.AnyAsync(p => p.Name == name.Trim());
        }

        return !await _dbContext.ProductBrands.Where(p => p.Id != id).AnyAsync(p => p.Name == name.Trim());
    }
}
