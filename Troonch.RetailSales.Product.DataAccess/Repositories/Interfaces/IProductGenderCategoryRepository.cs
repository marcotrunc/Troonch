using Troonch.Sales.Domain.Entities;

namespace Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces
{
    public interface IProductGenderCategoryRepository
    {
        Task<int> BulkDeleteByCategoryIdAsync(Guid categoryId);
        Task BulkDeleteByGenderIdAsync(Guid genderId);
        void Delete(ProductGenderCategoryLookup productGenderCategoryLookup);
        Task AddRangeAsync(List<ProductGenderCategoryLookup> productGenderCategoryLookups);
        Task<List<ProductGenderCategoryLookup>> GetAllAsync();
        Task<List<ProductGenderCategoryLookup>> GetProductGenderCategoryByCategoryIdAsync(Guid categoryId);
        Task<List<ProductGenderCategoryLookup>> GetProductGenderCategoryByGenderIdAsync(Guid genderId);
    }
}