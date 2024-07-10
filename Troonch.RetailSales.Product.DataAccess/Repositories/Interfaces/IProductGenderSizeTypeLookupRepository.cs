using Troonch.Sales.Domain.Entities;

namespace Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces
{
    public interface IProductGenderSizeTypeLookupRepository
    {
        Task<int> BulkDeleteByGenderIdAsync(Guid productGenderId);
        Task<int> BulkDeleteBySizeTypeIdAsync(Guid productSizeTypeId);
        void Delete(ProductGenderSizeTypeLookup record);
        Task<List<ProductGenderSizeTypeLookup>> GetAllAsync();
        Task<List<ProductGenderSizeTypeLookup>> GetGenderByProductSizeIdAsync(Guid productSizeId);
        Task<List<ProductGenderSizeTypeLookup>> GetSizeTypesByGenderIdAsync(Guid genderId);
    }
}