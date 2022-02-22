

namespace BlazorAppWebEcomm.Client.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            this._httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }
        public List<Product> Products { get; set ; }

        public async Task GetProducts()
        {
            var result = await _httpClient.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/Product");
            if (result != null && result.Data != null)
                Products = result.Data;
        }

        public async Task<ServiceResponse<Product>> GetProduct(int productId)
        {
           var result= await _httpClient.GetFromJsonAsync<ServiceResponse<Product>>($"api/Product/{productId}");
            return result;
        }
    }
}
