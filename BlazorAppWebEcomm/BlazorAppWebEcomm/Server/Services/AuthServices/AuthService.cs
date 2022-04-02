using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace BlazorAppWebEcomm.Server.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly ECommDatabaseContext context;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthService(ECommDatabaseContext context,IConfiguration configuration,IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.configuration = configuration;
            this.httpContextAccessor = httpContextAccessor;
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
                response.Data =CreateToken(user);
            }
            
            return response;
        }

        public async Task<ServiceResponse<int>> Register(Models.User user, string password)
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


        private bool VerifyPasswordHash(string password, byte[] passwordhash, byte[] passwordsalt)
        {
            using (var hmac = new HMACSHA512(passwordsalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordhash);
            }
        }

        private string  CreateToken(Models.User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.Email),
                new Claim(ClaimTypes.Role,user.Role)

            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token=new JwtSecurityToken(
                claims:claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;

        }

        public async Task<ServiceResponse<bool>> ChangePassword(int userId, string newPaswword)
        {
            var user =await context.Users.FindAsync(userId);
            if(user==null)
            {
                var response = new ServiceResponse<bool>()
                {
                    Success = false,
                    Message = "User not found."
                };
            }
            CreatePasswordHash(newPaswword, out byte[] passwordhash, out byte[] passwordSalt);
            user.PasswordHash = passwordhash;
            user.PasswordSalt = passwordSalt;
           await  context.SaveChangesAsync();
            return new ServiceResponse<bool>()
            {
                Data = true, 
                Message = "Password has been changed successfully"
            };
        }

        public int GetUserId()
        {
            try
            {
                return int.Parse(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            catch (Exception ex )
            {

                throw ex;
            }

        }

        public string GetUserEmail()
        {
            return httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        }

        public async Task<Models.User> GetUserByEmailId(string emailid)
        {
            return await context.Users.Where(p=>p.Email.Equals(emailid)).FirstOrDefaultAsync();
        }
    }
}
