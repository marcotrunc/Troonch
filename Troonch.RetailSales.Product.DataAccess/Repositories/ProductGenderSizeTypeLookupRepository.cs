using Microsoft.EntityFrameworkCore;
using Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces;
using Troonch.Sales.DataAccess;
using Troonch.Sales.Domain.Entities;

namespace Troonch.RetailSales.Product.DataAccess.Repositories;



public sealed class ProductGenderSizeTypeLookupRepository : IProductGenderSizeTypeLookupRepository
{
    private readonly RetailSalesProductDataContext _dbContext;
    public ProductGenderSizeTypeLookupRepository(RetailSalesProductDataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ProductGenderSizeTypeLookup>> GetAllAsync()
    {
        return await _dbContext
                        .ProductGenderSizeTypeLookup
                        .AsNoTracking()
                        .ToListAsync();

    }

    public async Task<List<ProductGenderSizeTypeLookup>> GetSizeTypesByGenderIdAsync(Guid genderId)
    {
        return await _dbContext
                        .ProductGenderSizeTypeLookup
                        .AsNoTracking()
                        .Where(pgsl => pgsl.ProductGenderId.Equals(genderId))
                        .ToListAsync();

    }

    public async Task<List<ProductGenderSizeTypeLookup>> GetGenderByProductSizeIdAsync(Guid productSizeId)
    {
        return await _dbContext
                        .ProductGenderSizeTypeLookup
                        .AsNoTracking()
                        .Where(pgsl => pgsl.ProductSizeTypeId.Equals(productSizeId))
                        .ToListAsync();

    }

    public void Delete(ProductGenderSizeTypeLookup record)
    {
        _dbContext.ProductGenderSizeTypeLookup.Remove(record);
    }

    public async Task<int> BulkDeleteBySizeTypeIdAsync(Guid productSizeTypeId)
    {
        return await _dbContext
                        .ProductGenderSizeTypeLookup
                        .AsNoTracking()
                        .Where(pgsl => pgsl.ProductSizeTypeId.Equals(productSizeTypeId))
                        .ExecuteDeleteAsync();
    }

    public async Task<int> BulkDeleteByGenderIdAsync(Guid productGenderId)
    {
        return await _dbContext
                        .ProductGenderSizeTypeLookup
                        .AsNoTracking()
                        .Where(pgsl => pgsl.ProductGenderId.Equals(productGenderId))
                        .ExecuteDeleteAsync();
    }
}
