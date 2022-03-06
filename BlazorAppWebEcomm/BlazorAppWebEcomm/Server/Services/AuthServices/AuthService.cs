using System.Security.Cryptography;

namespace BlazorAppWebEcomm.Server.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly EcommDatabaseContext context;

        public AuthService(EcommDatabaseContext context)
        {
            this.context = context;
        }

        public async Task<ServiceResponse<string>> Login(string email, string password)
        {
            var response = new ServiceResponse<string>();
            var user =await context.Users.Where(p => p.Email.ToLower().Equals(email.ToLower())).FirstOrDefaultAsync();
            if (user == null)
            {
                response.Success = false;
                response.Message = "User does not exists.";
            }
            else if (VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt) == false)
            {
                response.Success = false;
                response.Message = "Wrong password.";
            }
            else
            {
                response.Data = "token";
            }
            
            return response;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordhash, byte[] passwordsalt)
        {
            using (var hmac = new HMACSHA512(passwordsalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordhash);
            }
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            if (await UserExists(user.Email))
            {
                return new ServiceResponse<int>()
                {
                    Success = false,
                    Message = "User already exists."

                };
            }

            CreatePasswordHash(password, out byte[] passwordhash, out byte[] passwordsalt);
            user.PasswordHash = passwordhash;
            user.PasswordSalt = passwordsalt;
            context.Users.Add(user);
            try
            {
                await context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                
            }
            return new ServiceResponse<int>() { Data = user.Id , Message="Registration successful!"};
        }

        public async Task<bool> UserExists(string email)
        {
            if (await context.Users.AnyAsync(x => x.Email.ToLower().Equals(email.ToLower())))
            {
                return true;
            }
            return false;
        }
        private void CreatePasswordHash(string password, out byte[] passwordhash, out byte[] passwordsalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordsalt = hmac.Key;
                passwordhash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
