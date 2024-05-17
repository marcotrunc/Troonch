using Troonch.DataAccess.Base.Repositories;
using Troonch.Sales.Domain.Entities;

namespace Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces;
public interface IProductItemRepository : IBaseRepository<ProductItem>
{
    Task<IEnumerable<ProductItem>> GetProductItemsByProductIdAsync(Guid productId);
    Task<bool> IsUniqueBarcodeAsync(Guid? id, string barcode);
}