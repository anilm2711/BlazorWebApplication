namespace BlazorAppWebEcomm.Server.Services.CartServices
{
    public interface ICartService
    {
        Task<ServiceResponse<List<CartProductResponse>>> GetCartProducts(List<Models.CartItem> cartItems);
    }
}
