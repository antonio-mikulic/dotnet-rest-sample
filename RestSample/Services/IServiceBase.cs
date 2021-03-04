using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestSample.App.Services
{
    public interface IServiceBase<TEntity, TPrimaryKey> where TEntity : class
    {
        Task<TEntity> GetSingleAsync(TPrimaryKey id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> CreateAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
    }

    public interface IServiceBase<TEntity> : IServiceBase<TEntity, int> where TEntity : class
    {
    }
}