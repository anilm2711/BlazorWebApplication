namespace BlazorAppWebEcomm.Client.Services.CartService
{
    public interface ICartService
    {
        event Action OnChange;
        Task AddToCart(CartItem item);
        Task<List<CartProductResponse>> GetCartProducts();
        Task RemoveProductFromCart(int productId, int productTypeid);

        Task UpdateProductQuantity(CartProductResponse cartProductResponse);

        Task StoreCartItems(bool emptyLocalCart);
        Task GetCartItemsCount();


    }
}
