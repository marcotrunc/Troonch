namespace Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces
{
    public interface IProductSizeTypeRepository 
    {
        Task<bool> Exists(Guid id);
    }
}