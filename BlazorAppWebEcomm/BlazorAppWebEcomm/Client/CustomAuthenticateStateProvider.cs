using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace BlazorAppWebEcomm.Client
{
    public class CustomAuthenticateStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService localStorageService;
        private readonly HttpClient httpClient;

        public CustomAuthenticateStateProvider(ILocalStorageService localStorageService,HttpClient httpClient)
        {
            this.localStorageService = localStorageService;
            this.httpClient = httpClient;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string authToken =await localStorageService.GetItemAsStringAsync("authToken");
            var identity = new ClaimsIdentity();
            httpClient.DefaultRequestHeaders.Authorization = null;
            if(string.IsNullOrEmpty(authToken)==false)
            {
                try
                {
                    identity = new ClaimsIdentity(ParseClaimFromJWt(authToken), "jwt");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",authToken.Replace("\"",""));
                }
                catch 
                {
                    await localStorageService.RemoveItemAsync("authToken");
                    identity = new ClaimsIdentity();
                }
            }
            var user=new ClaimsPrincipal(identity);
            var state = new AuthenticationState(user);
            NotifyAuthenticationStateChanged(Task.FromResult(state));
            return state;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch(base64.Length%4)
            {
                case 2:base64 += "=="; break;
                case 3:base64 += "="; break; 
            }
            return Convert.FromBase64String(base64);
        }
        public IEnumerable<Claim> ParseClaimFromJWt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonbytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonbytes);
            var claims = keyValuePairs.Select(x => new Claim(x.Key, x.Value.ToString()));
            return claims;
        }
        
    }
}
