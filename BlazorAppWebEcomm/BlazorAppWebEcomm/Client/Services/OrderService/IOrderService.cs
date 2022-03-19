﻿namespace BlazorAppWebEcomm.Client.Services.OrderService
{
    public interface IOrderService
    {
        Task PlaceOrder();
        Task<List<OrderOverViewResponse>> GetOrders();
        Task<List<OrderDetailsResponse>> GetOrderDetails(int orderId);
    }
}
