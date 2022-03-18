using Microsoft.AspNetCore.Components;

namespace BlazorAppWebEcomm.Client.Services.OrderService
{
    public class OrderService:IOrderService
    {
        private readonly AuthenticationStateProvider authenticationStateProvider;
        private readonly NavigationManager navigationManager;
        private readonly HttpClient httpClient;

        private List<OrderOverViewResponse> orderOverViewResponses { get; set; }

        public OrderService(HttpClient httpClient,AuthenticationStateProvider authenticationStateProvider,NavigationManager navigationManager)
        {
            this.authenticationStateProvider = authenticationStateProvider;
            this.navigationManager = navigationManager;
            this.httpClient = httpClient;
        }

        private async Task<bool> IsUserAuthenticated()
        {
            return(await authenticationStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
        }

        public async Task PlaceOrder()
        {
            if(await IsUserAuthenticated())
            {
                await httpClient.PostAsync("api/order",null);
            }
            else
            {
                navigationManager.NavigateTo("login");
            }
        }

        public async Task<List<OrderOverViewResponse>> GetOrders()
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<List<OrderOverViewResponse>>>("api/order");
            return result.Data;
        }
    }
}
