using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApplication1.Products;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public ProductsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _httpClient = _httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://api.restful-api.dev/");
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string? name, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var products = await _httpClient.GetFromJsonAsync<List<Product>>("objects");

            if (products == null)
            {
                return NotFound("No products found.");
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                products = [.. products.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase))];
            }

            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            var totalItems = products.Count;
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var responseValue = products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(responseValue);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddProductRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"objects", request);

            response.EnsureSuccessStatusCode();

            var responseValue = await response.Content.ReadFromJsonAsync<AddProductResponse>();

            return StatusCode(201, responseValue!.Id);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"objects/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound($"Product with id {id} not found");
            }

            return Ok();
        }
    }
}
