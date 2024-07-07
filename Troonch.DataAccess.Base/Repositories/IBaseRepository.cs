using Troonch.Domain.Base.Entities;

namespace Troonch.DataAccess.Base.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        Task<List<TEntity>> GetAllAsync(string? searchTerm);
        Task<TEntity?> GetByIdAsync(Guid id);
        Task<TEntity?> AddAsync(TEntity entity);
        void Delete(TEntity entity);
        void Update(TEntity entity);
        Task<bool> IsExistingById(Guid id);
    }
}