using BlazorAppWebEcomm.Client.Services.CartService;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorAppWebEcomm.Client.Shared
{
    public partial class CartCounter : ComponentBase, IDisposable
    {
        [Inject]
        public ICartService cartService { get; set; }

        [Inject]
        public ISyncLocalStorageService localStorageService { get; set; }

        protected int GetCartItemsCount()
        {
            var cart = localStorageService.GetItem<List<CartItem>>("cart");
            return cart != null ? cart.Count : 0;
        }

        protected override void OnInitialized()
        {
            cartService.OnChange += StateHasChanged;
        }

        public void Dispose()
        {
            cartService.OnChange -= StateHasChanged;
        }
    }
}
