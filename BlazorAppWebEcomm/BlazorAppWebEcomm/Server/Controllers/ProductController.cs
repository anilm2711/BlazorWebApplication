using BlazorAppWebEcomm.Server.Models;
using BlazorAppWebEcomm.Server.Services.ProductServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppWebEcomm.Server.Controllers
{
    //[Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            this._productService = productService;
        }

        [HttpGet]
        [Route("api/Product/GetProducts")]
        public async Task<ActionResult<ServiceResponse<List<Models.Product>>>> GetProducts()
        {
            ServiceResponse<List<Models.Product>>? products = await _productService.GetProductsAsync();
            return Ok(products);
        }

        [HttpGet]
        [Route("api/Product/GetProduct/{productId}")]
        public async Task<ActionResult<ServiceResponse<Models.Product>>> GetProduct(int productId)
        {
            ServiceResponse<Models.Product>? product = await _productService.GetProductAsync(productId);
            return Ok(product);
        }

        [HttpGet]
        [Route("api/Product/GetProductsByCategory/{categoryUrl}")]
        public async Task<ActionResult<ServiceResponse<List<Models.Product>>>> GetProductsByCategory(string categoryUrl)
        {
            ServiceResponse<List<Models.Product>>? products = await _productService.GetProductByCategoryAsync(categoryUrl);
            return Ok(products);
        }

        [HttpGet]
        [Route("api/Product/SearchProducts/{searchText}")]
        public async Task<ActionResult<ServiceResponse<List<Models.Product>>>> SearchProducts(string searchText)
        {
            ServiceResponse<List<Models.Product>>? products = await _productService.SearchProducts(searchText);
            return Ok(products);
        }

        [HttpGet]
        [Route("api/Product/GetProductSearchSuggestion/{searchText}")]
        public async Task<ActionResult<ServiceResponse<List<string>>>> GetProductSearchSuggestion(string searchText)
        {
            ServiceResponse<List<string>>? strSearchSuggestion = await _productService.GetProductSearchSuggestion(searchText);
            return Ok(strSearchSuggestion);
        }


        [HttpGet]
        [Route("api/Product/GetFeaturedProductsAsync")]
        public async Task<ActionResult<ServiceResponse<List<Models.Product>>>> GetFeaturedProductsAsync()
        {
            ServiceResponse<List<Models.Product>>? products = await _productService.GetFeaturedProductsAsync();
            return Ok(products);
        }
    }
}
