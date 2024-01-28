namespace Dynatron.Shared
{
    public interface ISeedData<TEntity> where TEntity : EntityBase 
    {
        IEnumerable<TEntity> GetData();
    }
}
