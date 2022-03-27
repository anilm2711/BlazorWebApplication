﻿namespace BlazorAppWebEcomm.Client.Services.AddressService
{
    public class AddressService : IAddressService
    {
        private readonly HttpClient httpClient;

        public AddressService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<Address> AddOrUpdateAddress(Address address)
        {
            var response = await httpClient.PostAsJsonAsync("api/address/addorupdateaddress", address);
            var result = response.Content.ReadFromJsonAsync<ServiceResponse<Address>>().Result.Data;
            return result;
        }

        public async Task<Address> GetAddress()
        {
            var response = await httpClient.GetFromJsonAsync<ServiceResponse<Address>>("api/address");
            return response.Data;
        }
    }
}
