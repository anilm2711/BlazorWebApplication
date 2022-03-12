namespace BlazorAppWebEcomm.Client.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient httpClient;

        public AuthService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
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
    }
}
