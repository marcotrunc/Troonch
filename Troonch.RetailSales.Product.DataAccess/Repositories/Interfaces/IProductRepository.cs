using Troonch.DataAccess.Base.Helpers;
using Troonch.DataAccess.Base.Repositories;


namespace Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces
{
    public interface IProductRepository : IBaseRepository<SalesEntity.Product>
    {
        Task<bool> IsNameUniqueAync(Guid? id, string name);
        Task<IEnumerable<SalesEntity.Product>> GetProductsAsync(string? searchTerm);
        Task<SalesEntity.Product?> GetProductByIdAsync(Guid id);
        Task<IEnumerable<SalesEntity.Product>> GetProductsByCategoryIdAsync(Guid categoryId);
        Task<IEnumerable<SalesEntity.Product>> GetProductsByBrandIdAsync(Guid brandId);
        Task<IEnumerable<SalesEntity.Product>> GetProductsDeletedAsync();

        #region Ecommerce Product Repository
        Task<IEnumerable<SalesEntity.Product>> GetProductsPublishedAsync(string? searchTerm);
        Task<IEnumerable<SalesEntity.Product>> GetProductsPublishedByCategoryIdAsync(Guid categoryId);
        Task<IEnumerable<SalesEntity.Product>> GetProductsPublishedByBrandIdAsync(Guid brandId);
        Task<IEnumerable<SalesEntity.Product>> GetProductsPublishedByGenderId(Guid genderId);
        Task<SalesEntity.Product?> GetProductPublishedByIdAsync(Guid id);
        #endregion

    }
}