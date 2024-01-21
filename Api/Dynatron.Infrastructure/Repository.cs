using Dynatron.Shared;
using Microsoft.EntityFrameworkCore;

namespace Dynatron.Infrastructure
{
    public class Repository : IRepository
    {
        private readonly CustomerDbContext _dbContext;

        public Repository(CustomerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TEntity?> GetById<TEntity>(int id) where TEntity : EntityBase
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<IList<TEntity>> List<TEntity>() where TEntity : EntityBase
        {
            return await _dbContext.Set<TEntity>()
                .ToListAsync()
                .ContinueWith<IList<TEntity>>(t => t.Result, TaskContinuationOptions.ExecuteSynchronously);
        }

        public async Task<PagedList<TEntity>> List<TEntity>(PaginationCriteria criteria) where TEntity : EntityBase
        {
            var totalRows = await _dbContext.Set<TEntity>().CountAsync();
            var entities = await _dbContext.Set<TEntity>()
                .Skip(criteria.RowOffset)
                .Take(criteria.PageSize)
                .ToListAsync();

            return new PagedList<TEntity>(entities, criteria.Page, criteria.PageSize, totalRows);
        }

        public async Task Add<TEntity>(TEntity entity) where TEntity : EntityBase
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update<TEntity>(TEntity entity) where TEntity : EntityBase
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
