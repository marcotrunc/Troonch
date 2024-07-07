using Microsoft.EntityFrameworkCore;
using Troonch.DataAccess.Base.Helpers;
using Troonch.DataAccess.Base.Repositories;
using Troonch.Domain.Base.Entities;
using Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces;
using Troonch.Sales.DataAccess;

namespace Troonch.RetailSales.Product.DataAccess.Repositories
{
    public sealed class ProductRepository : BaseRepository<SalesEntity.Product, RetailSalesProductDataContext>, IProductRepository
    {
        public ProductRepository(RetailSalesProductDataContext dbContext) : base(dbContext)
        {
        }
        public async Task<bool> IsNameUniqueAync(Guid? id,string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return true;
            }

            if (id == null || id == Guid.Empty)
            {
                return !await _dbContext.Products.AnyAsync(p => p.Name == name.Trim());
            }

            return !await _dbContext.Products.Where(p=> p.Id != id).AnyAsync(p => p.Name == name.Trim());

        }

        public async Task<IEnumerable<SalesEntity.Product>> GetProductsAsync(string? searchTerm)
        {
            IQueryable<SalesEntity.Product> productsQuery = _dbContext.Products
                                        .AsNoTracking()
                                        .OrderByDescending(p => p.UpdatedOn)
                                        .Include(p => p.ProductGender)
                                        .Include(p => p.ProductBrand)
                                        .Include(p => p.ProductCategory)
                                        .Include(p => p.ProductMaterial);
                                        

            if (!string.IsNullOrEmpty(searchTerm) && !string.IsNullOrWhiteSpace(searchTerm))
            {
                productsQuery = productsQuery.Where(p =>
                    p.Name.Contains(searchTerm) ||
                    p.Description.Contains(searchTerm) ||
                    p.ProductBrand.Name.Contains(searchTerm) ||
                    p.ProductGender.Name.Contains(searchTerm) ||
                    p.ProductMaterial.Value.Contains(searchTerm) ||
                    p.ProductCategory.Name.Contains(searchTerm) ||
                    p.ProductItems.Any(
                        pi => 
                            pi.Barcode.Contains(searchTerm) ||
                            pi.Color.Contains(searchTerm)
                    ));
            }
         
            return await productsQuery.ToListAsync();  
        }

        public async Task<SalesEntity.Product?> GetProductByIdAsync(Guid id)
        {
            return await _dbContext.Products.AsNoTracking()
                .Include(p => p.ProductGender)
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductCategory)
                .Include(p => p.ProductMaterial)
                .SingleOrDefaultAsync(p => p.Id == id); 
        }
        public async Task<SalesEntity.Product?> GetProductBySlugAsync(string slug)
        {
            return await _dbContext.Products.AsNoTracking()
                .Include(p => p.ProductGender)
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductCategory)
                .Include(p => p.ProductMaterial)
                .SingleOrDefaultAsync(p => p.Slug == slug);
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
            public async Task<IEnumerable<SalesEntity.Product>> GetProductsPublishedAsync(string? searchTerm)
            {
            IQueryable<SalesEntity.Product> productsQuery = _dbContext.Products
                                    .AsNoTracking()
                                    .OrderByDescending(p => p.UpdatedOn)
                                    .Include(p => p.ProductGender)
                                    .Include(p => p.ProductBrand)
                                    .Include(p => p.ProductCategory)
                                    .Include(p => p.ProductMaterial)
                                    .Where(p => !p.IsDeleted && p.IsPublished == true)
                                    .Where(p => p.ProductItems.AsEnumerable().Any(pi => pi.QuantityAvailable > 0));

                if (!string.IsNullOrEmpty(searchTerm) && !string.IsNullOrWhiteSpace(searchTerm))
                {
                    productsQuery = productsQuery
                        .Where(p =>
                            p.Name.Contains(searchTerm) ||
                            p.Description.Contains(searchTerm) ||
                            p.ProductBrand.Name.Contains(searchTerm) ||
                            p.ProductGender.Name.Contains(searchTerm) ||
                            p.ProductMaterial.Value.Contains(searchTerm) ||
                            p.ProductCategory.Name.Contains(searchTerm) ||
                            p.ProductItems.Any(
                                pi =>
                                    pi.Barcode.Contains(searchTerm) ||
                                    pi.Color.Contains(searchTerm)
                            ));
                }

                return await productsQuery.ToListAsync();
            }
            public async Task<SalesEntity.Product?> GetProductPublishedByIdAsync(Guid id)
            {
                return await _dbContext.Products.AsNoTracking()
                       .OrderByDescending(p => p.UpdatedOn)
                       .Where(p => !p.IsDeleted && p.IsPublished == true)
                       .Where(p => p.ProductItems.AsEnumerable().Any(pi => pi.QuantityAvailable > 0))
                       .SingleOrDefaultAsync(p => p.Id == id);
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
           
        #endregion

    }
}
