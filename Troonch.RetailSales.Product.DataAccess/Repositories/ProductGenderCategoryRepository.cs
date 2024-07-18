using Microsoft.EntityFrameworkCore;
using Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces;
using Troonch.Sales.DataAccess;
using Troonch.Sales.Domain.Entities;

namespace Troonch.RetailSales.Product.DataAccess.Repositories;

public sealed class ProductGenderCategoryRepository : IProductGenderCategoryRepository
{
    private readonly RetailSalesProductDataContext _dbContext;
    public ProductGenderCategoryRepository(RetailSalesProductDataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ProductGenderCategoryLookup>> GetAllAsync()
    {
        return await _dbContext.ProductGenderCategoryLookup
                            .AsNoTracking()
                            .ToListAsync();
    }

    public async Task<List<ProductGenderCategoryLookup>> GetProductGenderCategoryByCategoryIdAsync(Guid categoryId)
    {
        if (categoryId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(categoryId));
        }

        return await _dbContext.ProductGenderCategoryLookup
            .AsNoTracking()
            .Where(pgcl => pgcl.ProductCategoryId.Equals(categoryId))
            .ToListAsync();
    }

    public async Task<List<ProductGenderCategoryLookup>> GetProductGenderCategoryByGenderIdAsync(Guid genderId)
    {
        if (genderId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(genderId));
        }

        return await _dbContext.ProductGenderCategoryLookup
            .AsNoTracking()
            .Where(pgcl => pgcl.ProductGenderId.Equals(genderId))
            .ToListAsync();
    }

    public async Task AddAsync(List<ProductGenderCategoryLookup> productGenderCategoryLookups)
    {
        await _dbContext.ProductGenderCategoryLookup
            .AddRangeAsync(productGenderCategoryLookups);
    }

    public void Delete(ProductGenderCategoryLookup productGenderCategoryLookup)
    {
        _dbContext.ProductGenderCategoryLookup.Remove(productGenderCategoryLookup);
    }

    public async Task BulkDeleteByCategoryIdAsync(Guid categoryId)
    {

        if (categoryId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(categoryId));
        }
        await _dbContext.ProductGenderCategoryLookup
            .AsNoTracking()
            .Where(pgcl => pgcl.ProductCategoryId.Equals(categoryId))
            .ExecuteDeleteAsync();
    }

    public async Task BulkDeleteByGenderIdAsync(Guid genderId)
    {

        if (genderId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(genderId));
        }

        await _dbContext.ProductGenderCategoryLookup
            .AsNoTracking()
            .Where(pgcl => pgcl.ProductGenderId.Equals(genderId))
            .ExecuteDeleteAsync();
    }

    
}
