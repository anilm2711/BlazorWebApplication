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
            var count = localStorageService.GetItem<int>("cartItemsCount");
            return count;
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
