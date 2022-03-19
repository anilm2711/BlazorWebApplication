namespace BlazorAppWebEcomm.Server.Services.OrderService
{
    public interface IOrderService
    {
        Task<ServiceResponse<bool>> PlaceOrder();
        Task<ServiceResponse<List<OrderOverViewResponse>>> GetOrders();
        Task<ServiceResponse<List<OrderDetailsResponse>>> GetOrderDetails(int orderId);


    }
}
