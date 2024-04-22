
using Microsoft.EntityFrameworkCore;
using Troonch.DataAccess.Base.Repositories;
using Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces;
using Troonch.Sales.DataAccess;

namespace Troonch.RetailSales.Product.DataAccess.Repositories;

public sealed class ProductGenderRepository : BaseRepository<SalesEntity.ProductGender, RetailSalesProductDataContext>, IProductGenderRepository
{
    public ProductGenderRepository(RetailSalesProductDataContext dbContext) : base(dbContext)
    {
    }
}
