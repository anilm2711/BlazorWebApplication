using BlazorAppWebEcomm.Client.Services.CartService;
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

        public List<CartProductResponse> cartProductResponses { get; set; } = null;
        string message = "Loading cart...";
        protected override async Task OnInitializedAsync()
        {
          await LoadCart();
        }

        public async Task RemoveProductFromCart(int productId, int? productTypeid)
        {
            await cartService.RemoveProductFromCart(productId, Convert.ToInt32(productTypeid));
            await LoadCart();
        }

        private async Task LoadCart()
        {
            if ((await cartService.GetCartItems()).Count() == 0)
            {
                message = "Your cart is empty.";
                cartProductResponses = new List<CartProductResponse>();
            }
            else
            {
                cartProductResponses = await cartService.GetCartProducts();
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
    }
}
