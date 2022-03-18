﻿using BlazorAppWebEcomm.Client.Services.CartService;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorAppWebEcomm.Client.Pages
{
    public partial class Cart : ComponentBase
    {
        [Inject]
        public ICartService  cartService { get; set; }

        [Inject]
        IOrderService orderService { get; set; }

        public List<CartProductResponse> cartProductResponses { get; set; } = null;
        string message = "Loading cart...";
        private bool IsOrderPalced = false;
        protected override async Task OnInitializedAsync()
        {
            IsOrderPalced = false;
          await LoadCart();
        }

        public async Task RemoveProductFromCart(int productId, int? productTypeid)
        {
            await cartService.RemoveProductFromCart(productId, Convert.ToInt32(productTypeid));
            await LoadCart();
        }

        private async Task LoadCart()
        {
            await cartService.GetCartItemsCount();
            cartProductResponses = await cartService.GetCartProducts();
            if (cartProductResponses == null || cartProductResponses.Count == 0)
            {
                message = "Your cart is empty.";
                cartProductResponses = new List<CartProductResponse>();
            }

        }

        public async Task UpdateQuantity(ChangeEventArgs e,CartProductResponse cartProductResponse)
        {
            cartProductResponse.Quantity = int.Parse(e.Value.ToString());
            if(cartProductResponse.Quantity<1)
            {
                cartProductResponse.Quantity = 1;
            }
            await cartService.UpdateProductQuantity(cartProductResponse);
        }

        private async Task PlaceOrder()
        {
            await orderService.PlaceOrder();
            await cartService.GetCartItemsCount();
            IsOrderPalced = true;
        }
    }
}
