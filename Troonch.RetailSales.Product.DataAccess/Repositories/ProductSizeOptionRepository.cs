using Microsoft.EntityFrameworkCore;
using Troonch.DataAccess.Base.Repositories;
using Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces;
using Troonch.Sales.DataAccess;
using Troonch.Sales.Domain.Entities;

namespace Troonch.RetailSales.Product.DataAccess.Repositories;

public sealed class ProductSizeOptionRepository : BaseRepository<ProductSizeOption, RetailSalesProductDataContext>, IProductSizeOptionRepository
{
    public ProductSizeOptionRepository(RetailSalesProductDataContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<ProductSizeOption>> GetProductSizeOptionsByTypeIdAsync(Guid typeId)
    {
        return await _dbContext.ProductSizeOptions.AsNoTracking()
                .Where(so => so.ProductSizeTypeId.Equals(typeId))
                .ToListAsync();
    }
}   
