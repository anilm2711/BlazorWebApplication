using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorAppWebEcomm.Client.Pages
{
    public partial class OrderDetails : ComponentBase
    {
        [Inject]
        IOrderService _derService { get; set; }

        [Parameter]
        public int orderId { get; set; }

        List<OrderDetailsResponse> orders  = new List<OrderDetailsResponse>();
        protected override async Task OnInitializedAsync()
        {
            orders = await _derService.GetOrderDetails(orderId);
        }
    }
}
