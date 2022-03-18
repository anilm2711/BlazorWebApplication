using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace BlazorAppWebEcomm.Client.Pages
{
    public partial class Order : ComponentBase
    {
        [Inject]
        public IOrderService orderService { get; set; }
        private List<OrderOverViewResponse> orders { get; set; }
        protected override async Task OnInitializedAsync()
        {
            orders = await orderService.GetOrders();
        }
    }
}
