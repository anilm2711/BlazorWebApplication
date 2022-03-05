﻿using Blazored.LocalStorage;

namespace BlazorAppWebEcomm.Client.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService localStorageService;
        private readonly HttpClient httpClient;

        public CartService(ILocalStorageService localStorageService,HttpClient httpClient)
        {
            this.localStorageService = localStorageService;
            this.httpClient = httpClient;
        }

        public event Action OnChange;

        public async Task AddToCart(CartItem item)
        {
            var cart = await localStorageService.GetItemAsync<List<CartItem>>("cart");
            if (cart == null)
            {
                cart = new List<CartItem>();
            }
            var sameitem=cart.Find(x=>x.ProductId==item.ProductId && x.ProductTypeId==item.ProductTypeId) ;
            if (sameitem == null)
            {
                cart.Add(item);
            }
            else
            {
                sameitem.Quantity += item.Quantity;
            }
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
                OnChange.Invoke();
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
    }
}
