using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using RestSample.App.Dtos.Products;
using RestSample.App.Services.Products;

namespace RestSample.App.Controllers.Products
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IProductService _productService;

        public ProductsController(ILogger<ProductsController> logger, IHttpClientFactory clientFactory, IProductService productService)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _productService = productService;
        }

        // Used for simple get which returns all data
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();

            return Ok(products);
        }

        // This will allow us to use /product/filter with get params
        // Metadata will be build from filtered products
        [HttpGet("filter")]
        public async Task<IActionResult> GetAllFiltered([FromQuery] ProductInput input)
        {
            // ASP.NET Doesn't expect comma separated input as size=small,medium, it should be size=small&size=medium
            // Its specified that this endpoint should expect a size=small,medium as input
            // Single value will be received in size, it should be split in in array

            input.Size = input.Size.FirstOrDefault()?.Split(',').ToArray();
            input.Highlight = input.Highlight.FirstOrDefault()?.Split(',').ToArray();

            // Metadata builder could be defined somewhere else
            var defaultMetadataBuilder = new MetadataBuilder
            {
                SkipMostCommonWords = 5,
                TakeNextMostCommonWords = 10,
                IncludeProducts = true
            };

            var products = await _productService.GetAllFilteredWithMetadata(input, defaultMetadataBuilder);

            return Ok(products);
        }

        // This will allow us to use /product/filter with get params
        // This will display metadata for all products
        [HttpGet("metadata")]
        public async Task<IActionResult> GetMetadata()
        {
            // Don't include products in response, only metadata is needed in this function
            var defaultMetadataBuilder = new MetadataBuilder
            {
                SkipMostCommonWords = 5,
                TakeNextMostCommonWords = 10,
                IncludeProducts = false
            };

            var metadata = await _productService.GetAllMetadata(defaultMetadataBuilder);

            return Ok(metadata);
        }

    }
}
