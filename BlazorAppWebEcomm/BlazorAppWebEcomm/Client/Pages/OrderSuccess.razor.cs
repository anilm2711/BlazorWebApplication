using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorAppWebEcomm.Client.Pages
{
    public partial class OrderSuccess : ComponentBase
    {
        [Inject]
        public ICartService cartService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await cartService.GetCartItemsCount();
        }

    }
}
