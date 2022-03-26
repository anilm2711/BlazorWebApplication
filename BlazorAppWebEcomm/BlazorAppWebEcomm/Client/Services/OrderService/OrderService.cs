using Microsoft.AspNetCore.Components;

namespace BlazorAppWebEcomm.Client.Services.OrderService
{
    public class OrderService:IOrderService
    {
        private readonly NavigationManager navigationManager;
        private readonly HttpClient httpClient;
        private readonly IAuthService authService;

        private List<OrderOverViewResponse> orderOverViewResponses { get; set; }

        public OrderService(HttpClient httpClient,IAuthService authService,NavigationManager navigationManager)
        {
            this.navigationManager = navigationManager;
            this.httpClient = httpClient;
            this.authService = authService;
        }

        private async Task<bool> IsUserAuthenticated()
        {
            return await authService.IsUserAuthenticated();
        }

        public async Task<string> PlaceOrder()
        {
            if(await IsUserAuthenticated())
            {
                var result=await httpClient.PostAsync("api/payment/checkout",null);
                string url =await result.Content.ReadAsStringAsync();
                return url;
            }
            else
            {
                return "login";
            }
        }

        public async Task<List<OrderOverViewResponse>> GetOrders()
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<List<OrderOverViewResponse>>>("api/order");
            return result.Data;
        }

        public async Task<List<OrderDetailsResponse>> GetOrderDetails(int orderId)
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<List<OrderDetailsResponse>>>($"api/order/{orderId}");
            return result.Data;
        }
    }
}
