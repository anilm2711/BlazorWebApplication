namespace BlazorAppWebEcomm.Server.Services.AddressService
{
    public class AddressService : IAddressService
    {
        private readonly EcommDatabaseContext context;
        private readonly IAuthService authService;

        public AddressService(EcommDatabaseContext context ,IAuthService authService)
        {
            this.context = context;
            this.authService = authService;
        }

        public async Task<ServiceResponse<Models.Address>> AddOrUpdateAddress(Models.Address address)
        {
            var response = new ServiceResponse<Models.Address>();
            var dbAddress = (await GetAddress()).Data;
            if (dbAddress==null)
            {
                var userId = authService.GetUserId();
                address.UserId = userId;
                context.Addresses.Add(address);
                response.Data = address;

            }
            else
            {
                dbAddress.FirstName = address.FirstName;
                dbAddress.LastName = address.LastName;
                dbAddress.City=address.City;
                dbAddress.State = address.State;
                dbAddress.Street = address.Street;
                dbAddress.Zip=address.Zip;
                dbAddress.Country=address.Country;
                response.Data = dbAddress;
            }
            await context.SaveChangesAsync();
            return response;
        }

        public async Task<ServiceResponse<Models.Address>> GetAddress()
        {
            var userId = authService.GetUserId();
            var address=await context.Addresses.FirstOrDefaultAsync(p=>p.UserId == userId);
            return new ServiceResponse<Models.Address> { Data = address };
        }
    }
}
