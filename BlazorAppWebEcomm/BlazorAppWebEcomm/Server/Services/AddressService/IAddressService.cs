namespace BlazorAppWebEcomm.Server.Services.AddressService
{
    public interface IAddressService
    {
        Task<ServiceResponse<Models.Address>> GetAddress();
        Task<ServiceResponse<Models.Address>> AddOrUpdateAddress(Models.Address address);
    }
}
