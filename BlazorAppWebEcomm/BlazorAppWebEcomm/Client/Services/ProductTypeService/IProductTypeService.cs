namespace BlazorAppWebEcomm.Client.Services.ProductTypeService
{
    public interface IProductTypeService
    {
        event Action OnChange;

        public List<ProductType> productTypes { get; set; }
        Task GetProductTypes();
        Task AddProductType(ProductType productType);
        Task UpdateProductType(ProductType productType);
        ProductType CreateNewProductType();
    }
}
