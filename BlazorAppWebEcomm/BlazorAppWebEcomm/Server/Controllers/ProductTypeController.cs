using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAppWebEcomm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ProductTypeController : ControllerBase
    {
        private readonly IProductTypeService productTypeService;

        public ProductTypeController(IProductTypeService productTypeService)
        {
            this.productTypeService = productTypeService;
        }

        [HttpGet("getproducttypes")]
        public async Task<ActionResult<ServiceResponse<List<Models.ProductType>>>> GetProductTypes()
        {
            var response = await productTypeService.GetProductTypes();
            return Ok(response);
        }

        [HttpPost("addproducttypes")]
        public async Task<ActionResult<ServiceResponse<List<Models.ProductType>>>> AddProductTypes(Models.ProductType productType)
        {
            var response = await productTypeService.AddProductTypes(productType);
            return Ok(response);
        }

        [HttpPut("updateproducttypes")]
        public async Task<ActionResult<ServiceResponse<List<Models.ProductType>>>> UpdateProductTypes(Models.ProductType productType)
        {
            var response = await productTypeService.UpdateProductTypes(productType);
            return Ok(response);
        }
    }
}
