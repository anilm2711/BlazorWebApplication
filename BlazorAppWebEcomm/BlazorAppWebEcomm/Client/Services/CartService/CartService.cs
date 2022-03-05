using Blazored.LocalStorage;

namespace BlazorAppWebEcomm.Client.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService localStorageService;

        public CartService(ILocalStorageService localStorageService)
        {
            this.localStorageService = localStorageService;
        }

        public event Action OnChange;

        public async Task AddToCart(CartItem item)
        {
            var cart = await localStorageService.GetItemAsync<List<CartItem>>("cart");
            if (cart == null)
            {
                cart = new List<CartItem>();
            }
            cart.Add(item);
            await localStorageService.SetItemAsync("cart", cart);
            OnChange.Invoke();
        }

        public async Task<List<CartItem>> GetCartItems()
        {
            var cart = await localStorageService.GetItemAsync<List<CartItem>>("cart");
            if (cart == null)
            {
                cart = new List<CartItem>();
            }
            return cart;
        }
    }
}
