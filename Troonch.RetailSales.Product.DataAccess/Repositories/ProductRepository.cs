using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using Troonch.DataAccess.Base.Repositories;
using Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces;
using Troonch.Sales.DataAccess;

namespace Troonch.RetailSales.Product.DataAccess.Repositories
{
    public sealed class ProductRepository : BaseRepository<SalesEntity.Product, RetailSalesProductDataContext>, IProductRepository
    {
        public ProductRepository(RetailSalesProductDataContext dbContext) : base(dbContext)
        {
        }
        public async Task<bool> IsNameUniqueAync(string name)
        {
            bool isUnique = !await _dbContext.Products.AnyAsync(p => p.Name == name);
            return isUnique;
        }

        public async Task<IEnumerable<SalesEntity.Product>> GetProductsByCategoryIdAsync(Guid categoryId)
        {
            return await _dbContext.Products.AsNoTracking()
                .OrderByDescending(p => p.UpdatedOn)
                .Where(p => !p.IsDeleted && p.ProductCategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<SalesEntity.Product>> GetProductsByBrandIdAsync(Guid brandId)
        {
            return await _dbContext.Products.AsNoTracking()
                .OrderByDescending(p => p.UpdatedOn)
                .Where(p => !p.IsDeleted && p.ProductBrandId == brandId)
                .ToListAsync();
        }

        public async Task<IEnumerable<SalesEntity.Product>> GetProductsDeletedAsync()
        {
            return await _dbContext.Products.AsNoTracking()
                .OrderByDescending(p => p.UpdatedOn)
                .Where(p => p.IsDeleted == true)
                .ToListAsync();
        }


        #region Ecommerce Product Repository
            public async Task<IEnumerable<SalesEntity.Product>> GetProductsPublishedAsync()
            {
                return await _dbContext.Products.AsNoTracking()
                    .OrderByDescending(p => p.UpdatedOn)
                    .Where(p => !p.IsDeleted && p.IsPublished == true)
                    .Where(p => p.ProductItems.AsEnumerable().Any(pi => pi.QuantityAvailable > 0))
                    .ToListAsync();
            }
            public async Task<IEnumerable<SalesEntity.Product>> GetProductsPublishedByCategoryIdAsync(Guid categoryId)
            {
                return await _dbContext.Products.AsNoTracking()
                    .OrderByDescending(p => p.UpdatedOn)
                    .Where(p => !p.IsDeleted && p.IsPublished == true && p.ProductCategoryId == categoryId)
                    .Where(p => p.ProductItems.AsEnumerable().Any(pi => pi.QuantityAvailable > 0))
                    .ToListAsync();
            }
            public async Task<IEnumerable<SalesEntity.Product>> GetProductsPublishedByBrandIdAsync(Guid brandId)
            {
                return await _dbContext.Products.AsNoTracking()
                    .OrderByDescending(p => p.UpdatedOn)
                    .Where(p => !p.IsDeleted && p.IsPublished == true && p.ProductBrandId == brandId)
                    .Where(p => p.ProductItems.AsEnumerable().Any(pi => pi.QuantityAvailable > 0))
                    .ToListAsync();
            }
            public async Task<IEnumerable<SalesEntity.Product>> GetProductsPublishedByGenderId(Guid genderId)
            {
                return await _dbContext.Products.AsNoTracking()
                    .OrderByDescending(p => p.UpdatedOn)
                    .Where(p => !p.IsDeleted && p.IsPublished == true && p.ProductGender.Id == genderId)
                    .Where(p => p.ProductItems.AsEnumerable().Any(pi => pi.QuantityAvailable > 0))
                    .ToListAsync();
            }
            public async Task<SalesEntity.Product?> GetProductPublishedByIdAsync(Guid id)
            {
                return await _dbContext.Products.AsNoTracking()
                       .OrderByDescending(p => p.UpdatedOn)
                       .Where(p => !p.IsDeleted && p.IsPublished == true)
                       .Where(p => p.ProductItems.AsEnumerable().Any(pi => pi.QuantityAvailable > 0))
                       .SingleOrDefaultAsync(p => p.Id == id);
            }
        #endregion

    }
}
