using BlazorAppWebEcomm.Server.Services.CartServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlazorAppWebEcomm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;

        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }

        [HttpPost("products")]
        public async Task<ActionResult<ServiceResponse<List<CartProductResponse>>>> GetCartProducts(List<Models.CartItem> cartItems)
        {
            var result=await cartService.GetCartProducts(cartItems);
            return Ok(result);
        }

        [HttpPost("storecartitems")]
        public async Task<ActionResult<ServiceResponse<List<CartProductResponse>>>> StoreCartItems(List<Models.CartItem> cartItems)
        {
            var result = await cartService.StoreCartItems(cartItems);
            return Ok(result);
        }

        [HttpGet("count")]
        public async Task<ActionResult<ServiceResponse<int>>> GetCartItemsCount()
        {
            return await cartService.GetCartItemsCount();
        }

        [HttpGet("getdbcartproducts")]
        public async Task<ActionResult<ServiceResponse<List<CartProductResponse>>>> GetDbCartProducts()
        {
            var result= await cartService.GetDbCartProducts();
            return Ok(result);
        }

        [HttpPost("addtocart")]
        public async Task<ActionResult<ServiceResponse<bool>>> AddToCart(Models.CartItem cartItem)
        {
            var result = await cartService.AddToCart(cartItem);
            return Ok(result);
        }

        [HttpPut("updatequantity")]
        public async Task<ActionResult<ServiceResponse<bool>>> UpdateQuantity(Models.CartItem cartItem)
        {
            var result = await cartService.UpdateQuantity(cartItem);
            return Ok(result);
        }

        [HttpDelete("{productId}/{productTypeId}")]
        public async Task<ActionResult<ServiceResponse<bool>>> RemoveItemfromCart(int productId, int productTypeId)
        {
            var result = await cartService.RemoveItemfromCart(productId, productTypeId);
            return Ok(result);
        }
    }
}
