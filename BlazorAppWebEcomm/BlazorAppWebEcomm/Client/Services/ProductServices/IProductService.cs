namespace BlazorAppWebEcomm.Client.Services.ProductServices
{
    public interface IProductService
    {
        event Action ProductsChanged;
        List<Product> Products { get; set; }

        public int CurrentPage { get; set; }
        public int PageCount { get; set; }

        public string LastSearchText { get; set; }
        string Message { get; set; }
        Task GetProducts();
        Task<ServiceResponse<Product>> GetProduct(int productId);

        Task SearchProducts(string  searchText,int page);

        Task GetProductByCategory(string categoryUrl);

        Task<List<string>> GetProductSearchSuggestion(string searchText);
    }
}
