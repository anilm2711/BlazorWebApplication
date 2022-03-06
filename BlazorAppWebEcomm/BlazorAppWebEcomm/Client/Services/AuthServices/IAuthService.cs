namespace BlazorAppWebEcomm.Client.Services.AuthServices
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> Register(UserRegister request);
    }
}
