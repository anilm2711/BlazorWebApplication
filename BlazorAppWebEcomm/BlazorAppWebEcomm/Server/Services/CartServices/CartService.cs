using System.Security.Claims;

namespace BlazorAppWebEcomm.Server.Services.CartServices
{
    public class CartService : ICartService
    {
        private readonly EcommDatabaseContext ecommDatabaseContext;
        private readonly IHttpContextAccessor httpContextAccessor;

        public CartService(EcommDatabaseContext ecommDatabaseContext,IHttpContextAccessor httpContextAccessor)
        {
            this.ecommDatabaseContext = ecommDatabaseContext;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<int>> GetCartItemsCount()
        {
            int count = (await ecommDatabaseContext.CartItems.Where(p => p.UserId == GetUserId()).ToListAsync()).Count;
            return new ServiceResponse<int>
            {
                Data = count,
            };
        }
        public int GetUserId()
        {
            return int.Parse(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        public async Task<ServiceResponse<List<CartProductResponse>>> GetCartProducts(List<Models.CartItem> cartItems)
        {
            var result = new ServiceResponse<List<CartProductResponse>>()
            {
                Data = new List<CartProductResponse>()
            };
            foreach (var item in cartItems)
            {
                var product = await ecommDatabaseContext.Products.Where(p => p.ProductId == item.ProductId).FirstOrDefaultAsync();
                if (product == null)
                    continue;

                var productVariant = await ecommDatabaseContext.ProductVariants.Where(p => p.ProductId == item.ProductId && p.ProductTypeId == item.ProductTypeId).Include(x => x.ProductType).FirstOrDefaultAsync();
                if (productVariant == null)
                    continue;
                var cartProduct = new CartProductResponse()
                {
                    ProductId = product.ProductId,
                    Title = product.Title,
                    ImageUrl = product.ImageUrl,
                    ProductTypeId = productVariant.ProductTypeId,
                    ProductType = productVariant.ProductType.Name,
                    Price = productVariant.Price,
                    Quantity = item.Quantity
                };
                result.Data.Add(cartProduct);
            }
            return result;
        }

        public async Task<ServiceResponse<List<CartProductResponse>>> StoreCartItems(List<Models.CartItem> cartItems)
        {
            cartItems.ForEach(p => p.UserId =GetUserId());
            ecommDatabaseContext.CartItems.AddRange(cartItems);
            await ecommDatabaseContext.SaveChangesAsync();
            return await GetDbCartProducts();
        }

        public async Task<ServiceResponse<List<CartProductResponse>>> GetDbCartProducts()
        {
            return await GetCartProducts(ecommDatabaseContext.CartItems.Where(p => p.UserId == GetUserId()).ToList());
        }
    }
}
