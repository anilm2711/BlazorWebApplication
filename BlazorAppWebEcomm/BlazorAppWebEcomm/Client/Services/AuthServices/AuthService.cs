namespace BlazorAppWebEcomm.Client.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient httpClient;
        private readonly AuthenticationStateProvider authentication;

        public AuthService(HttpClient httpClient,AuthenticationStateProvider authentication)
        {
            this.httpClient = httpClient;
            this.authentication = authentication;
        }
        public async Task<ServiceResponse<int>> Register(UserRegister request)
        {
            var result =await httpClient.PostAsJsonAsync("api/Auth/register", request);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();
        }

        public async Task<ServiceResponse<string>> Login(UserLogin userLogin)
        {
           var result=await httpClient.PostAsJsonAsync("api/Auth/login",userLogin);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<bool>> ChangePassword(UserChangePassword request)
        {
            var result = await httpClient.PostAsJsonAsync("api/Auth/changepassword", request.Password);
            return  await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }

        public async Task<bool> IsUserAuthenticated()
        {
            return (await authentication.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
        }
    }
}
