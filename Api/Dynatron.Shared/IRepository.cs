namespace Dynatron.Shared
{
    public interface IRepository
    {
        Task<TEntity?> GetById<TEntity>(int id) where TEntity : EntityBase;
        Task<IList<TEntity>> List<TEntity>() where TEntity : EntityBase;
        Task<PagedList<TEntity>> List<TEntity>(PaginationCriteria criteria) where TEntity : EntityBase;
        Task Add<TEntity>(TEntity entity) where TEntity : EntityBase;
        Task Update<TEntity>(TEntity entity) where TEntity : EntityBase;
    }
}
