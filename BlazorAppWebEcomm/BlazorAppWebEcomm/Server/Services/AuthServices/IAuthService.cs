using BlazorAppWebEcomm.Shared;

namespace BlazorAppWebEcomm.Server.Services.AuthServices
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> Register(Models.User user, string password);
        Task<bool> UserExists(string email);
        Task<ServiceResponse<string>> Login(string email, string password);
        Task<ServiceResponse<bool>> ChangePassword(int userId, string newPaswword);
        int GetUserId();
        string GetUserEmail();
        Task<Models.User> GetUserByEmailId(string emailid);
    }
}
