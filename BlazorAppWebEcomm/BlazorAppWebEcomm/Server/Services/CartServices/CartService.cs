namespace BlazorAppWebEcomm.Server.Services.CartServices
{
    public class CartService : ICartService
    {
        private readonly EcommDatabaseContext ecommDatabaseContext;

        public CartService(EcommDatabaseContext ecommDatabaseContext)
        {
            this.ecommDatabaseContext = ecommDatabaseContext;
        }

        public async Task<ServiceResponse<List<CartProductResponse>>> GetCartProducts(List<CartItem> cartItems)
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
                    Price = productVariant.Price
                };
                result.Data.Add(cartProduct);
            }
            return result;
        }

    }
}
