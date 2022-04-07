

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
        public int CurrentPage { get; set; } = 1;
        public int PageCount { get; set; } = 0;
        public string LastSearchText { get; set; } = string.Empty;
        public List<Product> AdminProducts { get; set; } = new List<Product>();

        public async Task GetProducts()
        {
            var result = await _httpClient.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/Product/getFeaturedProductsAsync");
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
            {
                Products = result.Data;
            }
            if (Products.Count == 0)
            {
                Message = "No Products found";
            }
            ProductsChanged?.Invoke();
        }

        public async Task SearchProducts(string searchText, int page)
        {
            LastSearchText = searchText;
            var result = await _httpClient.GetFromJsonAsync<ServiceResponse<ProductSearchResult<Product>>>($"api/Product/SearchProducts/{searchText}/{page}");
            CurrentPage = 1;
            PageCount = 0;
            if (result != null && result.Data != null)
            {
                Products = result.Data.Products;
                CurrentPage = result.Data.CurrentPage;
                PageCount=result.Data.Pages;
            }


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

        public async Task GetAdminProducts()
        {
            var result = await _httpClient.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/Product/admin");
            AdminProducts = result.Data;
            CurrentPage = 1;
            PageCount = 0;
            if (AdminProducts.Count == 0)
                Message = "No Procuts.";
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            var result = await _httpClient.PostAsJsonAsync("api/product/creteproduct", product);
            var newproduct = (await result.Content.ReadFromJsonAsync<ServiceResponse<Product>>()).Data;
            return newproduct;
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            var result = await _httpClient.PutAsJsonAsync("api/product/updateproduct", product);
            var poductupdated = (await result.Content.ReadFromJsonAsync<Product>());
            return poductupdated;
          
        }

        public async Task DeleteProductAsync(Product product)
        {
            var result = await _httpClient.DeleteAsync($"api/product/deleteproduct/{product.ProductId}");
        }
    }
}
