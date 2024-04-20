using Microsoft.EntityFrameworkCore;

namespace Troonch.Application.Base.UnitOfWork
{
    public class UnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext
    {
        private readonly TDbContext _dbcontext;

        public UnitOfWork(TDbContext dbcontext)
        {
            _dbcontext = dbcontext;
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
                return false;
            }
        }

        public async Task<bool> UpdateAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
        {
            _dbcontext.Attach(entity);

            _dbcontext.Entry(entity).State = EntityState.Modified;

            return await this.CommitAsync(cancellationToken);
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