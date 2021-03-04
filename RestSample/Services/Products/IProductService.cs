using System.Collections.Generic;
using System.Threading.Tasks;
using RestSample.App.Dtos.Products;
using RestSample.Data.Models.Products;

namespace RestSample.App.Services.Products
{
    public interface IProductService<TPrimaryKey> : IServiceBase<Product, TPrimaryKey>
    {
        Task<IEnumerable<Product>> GetAllFilteredAsync(ProductInput input);

        Task<ProductOutput> GetAllMetadata(MetadataBuilder builder);
        Task<ProductOutput> GetAllFilteredWithMetadata(ProductInput input, MetadataBuilder builder);
    }

    public interface IProductService : IProductService<int>
    {
    }
}