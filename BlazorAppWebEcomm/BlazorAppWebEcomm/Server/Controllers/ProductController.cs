using BlazorAppWebEcomm.Server.Models;
using BlazorAppWebEcomm.Server.Services.ProductServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppWebEcomm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            this._productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Models.Product>>>> GetProducts()
        {
            ServiceResponse<List<Models.Product>>? products = await _productService.GetProductsAsync();
            return Ok(products);
        }

        [HttpGet]
        [Route("{productId}")]
        public async Task<ActionResult<ServiceResponse<Models.Product>>> GetProduct(int productId)
        {
            ServiceResponse<Models.Product>? product = await _productService.GetProductAsync(productId);
            return Ok(product);
        }
    }
}
