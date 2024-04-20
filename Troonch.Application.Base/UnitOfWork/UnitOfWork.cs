using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Troonch.Application.Base.UnitOfWork
{
    public class UnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext
    {
        private readonly TDbContext _dbcontext;
        private readonly ILogger<UnitOfWork<TDbContext>> _logger;

        public UnitOfWork(TDbContext dbcontext, ILogger<UnitOfWork<TDbContext>> logger)
        {
            _dbcontext = dbcontext;
            _logger = logger;
        }

        public async Task<bool> CommitAsync(CancellationToken cancellationToken)
        {
            try
            {
                SetTimestamps();
                return await _dbcontext.SaveChangesAsync(cancellationToken) > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<bool> UpdateAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
        {
            _dbcontext.Attach(entity);

            _dbcontext.Entry(entity).State = EntityState.Modified;

            bool isCommited = await this.CommitAsync(cancellationToken);
            if(isCommited)
            {
                _dbcontext.Entry(entity).State = EntityState.Detached;
                _dbcontext.ChangeTracker.Clear();
            }

            return isCommited;
        }

        private void SetTimestamps()
        {
            var entities = _dbcontext.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);


            var currentTime = DateTime.UtcNow;

            foreach ( var entity in entities)
            {
                try
                {
                    if (entity.Property("CreatedOn") == null)
                    {
                        continue;
                    }
                    if (entity.State == EntityState.Added)
                    {
                        entity.Property("CreatedOn").CurrentValue = currentTime;
                    }

                    if (entity.Property("UpdatedOn") == null)
                    {
                        continue;
                    }

                    entity.Property("UpdatedOn").CurrentValue = currentTime;
                }
                catch(Exception ex ) 
                {
                    continue;
                }
            }

        }
    }
}