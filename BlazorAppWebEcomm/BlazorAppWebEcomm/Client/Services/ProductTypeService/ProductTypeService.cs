namespace BlazorAppWebEcomm.Client.Services.ProductTypeService
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly HttpClient httpClient;

        public ProductTypeService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public List<ProductType> productTypes { get; set; } = new List<ProductType>();

        public event Action OnChange;

        public async Task AddProductType(ProductType productType)
        {
            var responseMessage = await httpClient.PostAsJsonAsync("api/producttype/addproducttypes", productType);
            productTypes = (await responseMessage.Content.ReadFromJsonAsync<ServiceResponse<List<ProductType>>>()).Data;
            OnChange.Invoke();
        }

        public ProductType CreateNewProductType()
        {
            var newProductType = new ProductType { IsNew = true, Editing = true };
            productTypes.Add(newProductType);
            OnChange.Invoke();
            return newProductType;
        }

        public async Task GetProductTypes()
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<List<ProductType>>>("api/ProductType/getproducttypes");
            productTypes = result.Data;
        }

        public async Task UpdateProductType(ProductType productType)
        {
            var response =await  httpClient.PutAsJsonAsync("api/producttype/updateproducttypes", productType);
            productTypes = (await response.Content.ReadFromJsonAsync<ServiceResponse<List<ProductType>>>()).Data;
            OnChange.Invoke();
        }
    }
}
