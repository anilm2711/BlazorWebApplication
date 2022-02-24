

namespace BlazorAppWebEcomm.Client.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        public event Action ProductsChanged;

        public ProductService(HttpClient httpClient)
        {
            this._httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }
        public List<Product> Products { get; set; }

        public async Task GetProducts()
        {
            var result = await _httpClient.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/Product/GetProducts");
            if (result != null && result.Data != null)
                Products = result.Data;
            ProductsChanged.Invoke();
        }

        public async Task<ServiceResponse<Product>> GetProduct(int productId)
        {
            var result = await _httpClient.GetFromJsonAsync<ServiceResponse<Product>>($"api/Product/GetProduct/{productId}");
            return result;
        }

        public async Task GetProductByCategory(string categoryUrl)
        {
            var result = await _httpClient.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/Product/GetProductsByCategory/{categoryUrl}");
            if (result != null && result.Data != null)
                Products = result.Data;
            ProductsChanged.Invoke();
        }

    }
}
