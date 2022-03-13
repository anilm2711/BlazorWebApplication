namespace BlazorAppWebEcomm.Server.Services.CartServices
{
    public interface ICartService
    {
        Task<ServiceResponse<List<CartProductResponse>>> GetCartProducts(List<Models.CartItem> cartItems);
        Task<ServiceResponse<List<CartProductResponse>>> StoreCartItems(List<Models.CartItem> cartItems);
        Task<ServiceResponse<int>> GetCartItemsCount();


    }
}
