namespace BlazorAppWebEcomm.Server.Services.ProductTypeServices
{
    public interface IProductTypeService
    {
        Task<ServiceResponse<List<Models.ProductType>>> GetProductTypes();
        Task<ServiceResponse<List<Models.ProductType>>> AddProductTypes(Models.ProductType productType);
        Task<ServiceResponse<List<Models.ProductType>>> UpdateProductTypes(Models.ProductType productType);
    }
}
