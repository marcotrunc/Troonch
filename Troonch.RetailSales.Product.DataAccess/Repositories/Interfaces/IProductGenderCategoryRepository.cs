using Troonch.Sales.Domain.Entities;

namespace Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces
{
    public interface IProductGenderCategoryRepository
    {
        Task BulkDeleteByCategoryId(Guid categoryId);
        Task BulkDeleteByGenderId(Guid genderId);
        void Delete(ProductGenderCategoryLookup productGenderCategoryLookup);
        Task<List<ProductGenderCategoryLookup>> GetAllAsync();
        Task<List<ProductGenderCategoryLookup>> GetProductGenderCategoryByCategoryIdAsync(Guid categoryId);
        Task<List<ProductGenderCategoryLookup>> GetProductGenderCategoryByGenderIdAsync(Guid genderId);
    }
}