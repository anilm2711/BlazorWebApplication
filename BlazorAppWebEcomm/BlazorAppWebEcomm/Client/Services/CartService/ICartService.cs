namespace BlazorAppWebEcomm.Client.Services.CartService
{
    public interface ICartService
    {
        event Action OnChange;
        Task AddToCart(CartItem item);

        Task<List<CartItem>> GetCartItems();

        Task<List<CartProductResponse>> GetCartProducts();
        Task RemoveProductFromCart(int productId, int productTypeid);
    }
}
