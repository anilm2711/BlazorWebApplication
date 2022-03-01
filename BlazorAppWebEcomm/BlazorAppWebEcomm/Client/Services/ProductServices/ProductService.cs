

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
        public string Message { get; set; }

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
            ProductsChanged?.Invoke();
        }

        public async Task SearchProducts(string searchText)
        {
            var result = await _httpClient.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/Product/SearchProducts/{searchText}");
            if (result != null && result.Data != null)
                Products = result.Data;
            if (Products.Count == 0)
            {
                Message = "No Products found";
            }
            ProductsChanged?.Invoke();
        }

        public async Task<List<string>> GetProductSearchSuggestion(string searchText)
        {
            var result = await _httpClient.GetFromJsonAsync<ServiceResponse<List<string>>>($"api/Product/GetProductSearchSuggestion/{searchText}");
            return result.Data;
        }
    }
}
