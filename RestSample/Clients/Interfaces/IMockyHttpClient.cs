using System.Threading.Tasks;
using RestSample.Core.Mocky;

namespace RestSample.App.Clients.Interfaces
{
    public interface IMockyHttpClient<TEntity> where TEntity : class
    {
        Task<MockyResultWrapper<TEntity>> BuildUrlAndGetAllWithCache(string endpoint);

        Task<MockyResultWrapper<TEntity>> GetAllWithCachedAsync(string url);

        Task<MockyResultWrapper<TEntity>> GetAllAsync(string url);

        Task<MockyResultWrapper<TEntity>> BuildUrlAndGetAllAsync(string endpoint);

        string BuildUrl(string endpoint);
    }
}
