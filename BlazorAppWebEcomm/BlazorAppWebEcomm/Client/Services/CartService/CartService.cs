using Blazored.LocalStorage;

namespace BlazorAppWebEcomm.Client.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService localStorageService;
        private readonly HttpClient httpClient;
        private readonly AuthenticationStateProvider authenticationStateProvider;

        public CartService(ILocalStorageService localStorageService,HttpClient httpClient,AuthenticationStateProvider authenticationStateProvider)
        {
            this.localStorageService = localStorageService;
            this.httpClient = httpClient;
            this.authenticationStateProvider = authenticationStateProvider;
        }

        public event Action OnChange;

        public async Task AddToCart(CartItem item)
        {
            if (await IsUserAuthenticated())
            {

            }
            var cart = await localStorageService.GetItemAsync<List<CartItem>>("cart");
            if (cart == null)
            {
                cart = new List<CartItem>();
            }
            var sameitem = cart.Find(x => x.ProductId == item.ProductId && x.ProductTypeId == item.ProductTypeId);
            if (sameitem == null)
            {
                cart.Add(item);
            }
            else
            {
                sameitem.Quantity += item.Quantity;
            }
            await localStorageService.SetItemAsync("cart", cart);
            GetCartItemsCount();
        }

        private async Task<bool> IsUserAuthenticated()
        {
            return (await authenticationStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
        }

        public async Task<List<CartItem>> GetCartItems()
        {
            GetCartItemsCount();
            var cart = await localStorageService.GetItemAsync<List<CartItem>>("cart");
            if (cart == null)
            {
                cart = new List<CartItem>();
            }
            return cart;
        }

        public async Task<List<CartProductResponse>> GetCartProducts()
        {
            var cart = await localStorageService.GetItemAsync<List<CartItem>>("cart");
            var response =await httpClient.PostAsJsonAsync("api/cart/products", cart);
            var cartProducts = await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartProductResponse>>>();
            return cartProducts.Data;
        }

        public async Task RemoveProductFromCart(int productId, int productTypeid)
        {
            var cart = await localStorageService.GetItemAsync<List<CartItem>>("cart");
            if (cart == null)
            {
                return;
            }
            var cartItem = cart.Find(x => x.ProductId == productId && x.ProductTypeId == productTypeid);
            if (cartItem != null)
            {
                cart.Remove(cartItem);
                await localStorageService.SetItemAsync("cart", cart);
                GetCartItemsCount();
            }
        }

        public async Task StoreCartItems(bool emptyLocalCart=false)
        {
            var cart = await localStorageService.GetItemAsync<List<CartItem>>("cart");
            if (cart == null)
            {
                return;
            }
           var response= httpClient.PostAsJsonAsync("api/cart/storecartitems", cart);
            if(emptyLocalCart==true)
            {
                await localStorageService.RemoveItemAsync("cart");
            }    

        }

        public async Task UpdateProductQuantity(CartProductResponse cartProductResponse)
        {
            var cart = await localStorageService.GetItemAsync<List<CartItem>>("cart");
            if (cart == null)
            {
                return;
            }
            var cartItem = cart.Find(x => x.ProductId == cartProductResponse.ProductId && x.ProductTypeId == cartProductResponse.ProductTypeId);
            if (cartItem != null)
            {
                cartItem.Quantity=cartProductResponse.Quantity;
                await localStorageService.SetItemAsync("cart", cart);
            }
        }

        public async Task GetCartItemsCount()
        {
            if (await IsUserAuthenticated())
            {
                var result = await httpClient.GetFromJsonAsync<ServiceResponse<int>>("api/cart/count");
                int count = result.Data;
                await localStorageService.SetItemAsync<int>("cartItemsCount", count);
            }
            else
            {
                var cart =await localStorageService.GetItemAsync<List<CartItem>>("cart");
                await localStorageService.SetItemAsync<int>("cartItemsCount", cart != null ? cart.Count : 0);
            }
            OnChange.Invoke();
        }
    }
}
