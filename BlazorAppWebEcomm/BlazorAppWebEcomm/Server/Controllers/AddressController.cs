using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAppWebEcomm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService addressService;

        public AddressController(IAddressService addressService)
        {
            this.addressService = addressService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<Models.Address>>> GetAddress()
        {
            return await addressService.GetAddress();
        }
        [HttpPost("addorupdateaddress")]
        public async Task<ActionResult<ServiceResponse<Models.Address>>> AddOrUpdateAddress(Models.Address address)
        {
            return await addressService.AddOrUpdateAddress(address);
        }

    }
}
