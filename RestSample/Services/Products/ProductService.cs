using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSample.App.Clients.Interfaces;
using RestSample.App.Controllers.Products;
using RestSample.App.Dtos.Products;
using RestSample.Core.Helpers;
using RestSample.Core.Mocky;
using RestSample.Data.Models.Products;

namespace RestSample.App.Services.Products
{
    // Working with Application layer so Service was placed in .App project
    public class ProductService : IProductService
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IMockyHttpClient<Product> _productClient;
        private readonly IOptions<MockyOptions> _options;

        public ProductService(ILogger<ProductsController> logger, IMockyHttpClient<Product> productClient, IOptions<MockyOptions> options)
        {
            _logger = logger;
            _productClient = productClient;
            _options = options;
        }

        // Gets a list of all products filtered by input
        public async Task<IEnumerable<Product>> GetAllFilteredAsync(ProductInput input)
        {
            var products = await GetAllAsync();

            products = FilterProducts(products, input as ProductInput);
            return products;
        }

        // Gets all products and metadata
        public async Task<ProductOutput> GetAllMetadata(MetadataBuilder builder)
        {
            var products = await GetAllAsync();
            return BuildMetadata(products, builder);
        }

        // Gets all products and metadata for those products
        public async Task<ProductOutput> GetAllFilteredWithMetadata(ProductInput input, MetadataBuilder builder)
        {
            var products = await GetAllFilteredAsync(input);
            return BuildMetadata(products, builder);
        }

        public Task<Product> CreateAsync(Product entity)
        {
            throw new NotImplementedException();
        }

        // This will return empty list even if call fails
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var products = await _productClient.BuildUrlAndGetAllWithCache(_options.Value.ProductsPage);

            return products.Products;
        }

        public Task<Product> GetSingleAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Product> UpdateAsync(Product entity)
        {
            throw new NotImplementedException();
        }

        public ProductOutput BuildMetadata(IEnumerable<Product> products, MetadataBuilder builder)
        {
            // Avoid multiple enumeration
            var productArray = products as Product[] ?? products.ToArray();

            if (productArray.Length == 0)
                return new ProductOutput();

            var output = new ProductOutput
            {
                Products = builder.IncludeProducts ? productArray.ToList() : new List<Product>(),
                MaxPrice = productArray.Max(s => s.Price),
                MinPrice = productArray.Min(s => s.Price),
                AllSizes = WordHelper.GetAllDistinctWordsFromListsOfWords(productArray.Select(s => s.Sizes)),
                CommonWords = WordHelper.BuildCommonWords(productArray.Select(s => s.Description), builder.SkipMostCommonWords, builder.TakeNextMostCommonWords)
            };

            var productsJson = JsonConvert.SerializeObject(products);
            var builderJson = JsonConvert.SerializeObject(builder);
            var foundMetadataJson = JsonConvert.SerializeObject(output);

            _logger.LogInformation($"Found metadata: {foundMetadataJson} for builder {builderJson} using product list {productsJson}");

            return output;
        }

        // Filter based in input
        // If this was database a IQueryable would be passed instead of IEnumerable
        private IEnumerable<Product> FilterProducts(IEnumerable<Product> products, ProductInput input)
        {
            var productArray = products as Product[] ?? products.ToArray();

            var initProductCount = productArray.Count();
            _logger.LogInformation($"Filtering {initProductCount} products.");

            // Checking if input exists manually... WhereIf extension could be created to bypass this

            if (input.MinPrice > 0)
                productArray = productArray.Where(s => s.Price >= input.MinPrice).ToArray();

            if (input.MaxPrice > 0)
                productArray = productArray.Where(s => s.Price <= input.MaxPrice).ToArray();

            if (input.Size != null && input.Size.Length > 0)
                productArray = productArray.Where(p => p.Sizes.Any(size => input.Size.Contains(size))).ToArray();

            if (input.Highlight != null && input.Highlight.Length > 0)
                foreach (var product in productArray)
                    product.Description = WordHelper.HighlightWord(product.Description, input.Highlight);

            var inputAsJson = JsonConvert.SerializeObject(input);

            _logger.LogInformation($"Finished filtering! Total products received from api: {initProductCount}. Filtered down to {productArray.Length} products using input: {inputAsJson}");
            return productArray;
        }
    }
}
