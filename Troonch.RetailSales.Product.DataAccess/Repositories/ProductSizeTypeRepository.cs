global using SalesEntity = Troonch.Sales.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Troonch.DataAccess.Base.Repositories;
using Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces;
using Troonch.Sales.DataAccess;

namespace Troonch.RetailSales.Product.DataAccess.Repositories;

public sealed class ProductSizeTypeRepository : BaseRepository<SalesEntity.ProductSizeType, RetailSalesProductDataContext>, IProductSizeTypeRepository
{
    public ProductSizeTypeRepository(RetailSalesProductDataContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> Exists(Guid id)
    {
        return await _dbContext.ProductSizeTypes.AsNoTracking().AnyAsync(pst => pst.Id == id);
    }
}
