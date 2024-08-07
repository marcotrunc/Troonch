﻿using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Troonch.DataAccess.Base.Helpers;
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
        public async Task<List<TEntity>> GetAllAsync(string? searchTerm)
        {
            IQueryable<TEntity> query = _dbContext
                                        .Set<TEntity>()
                                        .OrderByDescending(x => x.UpdatedOn)
                                        .AsNoTracking();

            var properties = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            if (!String.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.SearchEntities(searchTerm);
            }

            return await query.ToListAsync();
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

        public async Task<bool> IsExistingById(Guid id)
        {
            return await _dbContext.Set<TEntity>().AsNoTracking().AnyAsync(e => e.Id == id);
        }
        
    }
}
