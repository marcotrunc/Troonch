using Troonch.DataAccess.Base.Repositories;
using Troonch.Sales.Domain.Entities;

namespace Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces;

public interface IProductCategoryRepository : IBaseRepository<ProductCategory>
{
    Task<bool> IsUniqueNameAsync(Guid? id,string name);
    Task<IEnumerable<ProductCategory>> GetAllProductCategoriesWithSizeAsync(string searchTerm);
    Task<ProductCategory?> GetCategoryWithSizeAsync(Guid id);
}