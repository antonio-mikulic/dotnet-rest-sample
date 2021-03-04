using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using RestSample.App.Clients.Interfaces;
using RestSample.Core.Exceptions;
using RestSample.Core.Mocky;

namespace RestSample.App.Clients
{
    public class MockyHttpClient<TEntity> : IMockyHttpClient<TEntity> where TEntity : class
    {
        private readonly ILogger<MockyHttpClient<TEntity>> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IMemoryCache _memoryCache;
        private readonly IOptions<MockyOptions> _options;

        public MockyHttpClient(ILogger<MockyHttpClient<TEntity>> logger, IHttpClientFactory clientFactory, IOptions<MockyOptions> options, IMemoryCache memoryCache)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _memoryCache = memoryCache;
            _options = options;
        }

        public async Task<MockyResultWrapper<TEntity>> BuildUrlAndGetAllWithCache(string endpoint)
        {
            var url = BuildUrl(endpoint);
            return await GetAllWithCachedAsync(url);
        }

        // Use simple memory cache to store URL response
        public async Task<MockyResultWrapper<TEntity>> GetAllWithCachedAsync(string url)
        {
            var cacheEntry = await _memoryCache.GetOrCreateAsync(url, entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromSeconds(_options.Value.DefaultCacheDuration);
                    return GetAllAsync(url);
                });

            return cacheEntry;
        }

        // Create URL link
        public string BuildUrl(string endpoint)
        {
            return $"{_options.Value.BaseUrl.TrimEnd('/')}/{endpoint.TrimStart('/')}";
        }

        public async Task<MockyResultWrapper<TEntity>> BuildUrlAndGetAllAsync(string endpoint)
        {
            var uri = BuildUrl(endpoint);
            return await GetAllAsync(uri);
        }

        public async Task<MockyResultWrapper<TEntity>> GetAllAsync(string url)
        {
            try
            {
                // CreateAsync new Get request
                var request = new HttpRequestMessage(HttpMethod.Get, url);

                var client = _clientFactory.CreateClient();

                var response = await client.SendAsync(request);

                // Throw exception if failed to get response from uri
                if (!response.IsSuccessStatusCode) throw new Exception("Failed to get successful response");

                // Convert response to mocky result wrapper of Uri
                var responseStream = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<MockyResultWrapper<TEntity>>(responseStream);

                ValidateApiKeys(responseObject.ApiKeys);

                _logger.LogInformation($"Successfully got response: {responseStream}");
                return responseObject;
            }
            catch (Exception e)
            {
                // TODO -> Create a UserFriendlyExceptions which will catch exception and show custom message to user
                // For demo api purposes returning empty list is ok

                _logger.LogError($"Failed to get all entities from {url}. Got exception: {e}");
                return new MockyResultWrapper<TEntity>();
            }
        }

        // Sample for api key validation
        // Custom validation could be implemented, different responses used for primary and secondary etc.
        // Checks that at least one received key is in our 
        private void ValidateApiKeys(ApiKeysWrapper receivedKeys)
        {
            var isPrimaryOk = CheckIsApiKeyOk(receivedKeys.Primary, _options.Value.PrimaryApiKey);
            var isSecondaryOk = CheckIsApiKeyOk(receivedKeys.Secondary, _options.Value.SecondaryApiKey);

            if (!isPrimaryOk && !isSecondaryOk)
                throw new InvalidApiKeysException("Received invalid API keys");
        }

        // Received and expected keys should both be set and be equals
        private bool CheckIsApiKeyOk(string received, string expected)
        {
            return !String.IsNullOrEmpty(received) && !String.IsNullOrEmpty(expected) && received.Equals(expected);
        }
    }
}
