using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troonch.Domain.Base.Entities;

namespace Troonch.DataAccess.Base.Repositories
{
    public abstract class BaseRepository<TEntity, TDbContext> : IBaseRepository<TEntity> where TEntity : BaseEntity
        where TDbContext : DbContext
    {
        protected readonly TDbContext _dbContext;
        protected BaseRepository(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync();
        }
        public async Task<TEntity?> GetByIdAsync(Guid id)
        {
            return  await _dbContext.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        }
        public async Task<TEntity?> AddAsync(TEntity entity)
        {
            var entityAdded = await _dbContext.Set<TEntity>().AddAsync(entity);
            return entityAdded?.Entity;
        }

        public void Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
        }
    }
}
