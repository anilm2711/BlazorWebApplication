﻿using System.Security.Claims;

namespace BlazorAppWebEcomm.Server.Services.CartServices
{
    public class CartService : ICartService
    {
        private readonly ECommDatabaseContext ecommDatabaseContext;
        private readonly IAuthService authService;

        public CartService(ECommDatabaseContext ecommDatabaseContext,IAuthService authService)
        {
            this.ecommDatabaseContext = ecommDatabaseContext;
            this.authService = authService;
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
            return authService.GetUserId();
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
                    ProductTypeId = productVariant.ProductTypeId.Value,
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

        public async Task<ServiceResponse<List<CartProductResponse>>> GetDbCartProducts(int? userId = null)
        {
            if (userId == null)
                userId = GetUserId();
            return await GetCartProducts(ecommDatabaseContext.CartItems.Where(p => p.UserId == userId).ToList());
        }

        public async Task<ServiceResponse<bool>> AddToCart(Models.CartItem cartItem)
        {
            try
            {
                cartItem.UserId = GetUserId();
                var sameItem = await ecommDatabaseContext.CartItems.FirstOrDefaultAsync(p => p.ProductId == cartItem.ProductId && p.ProductTypeId == cartItem.ProductTypeId
                   && p.UserId == cartItem.UserId);
                if (sameItem == null)
                {
                    ecommDatabaseContext.CartItems.Add(cartItem);
                }
                else
                {
                    sameItem.Quantity += cartItem.Quantity;
                }
                await ecommDatabaseContext.SaveChangesAsync();
                return new ServiceResponse<bool>() { Data = true };
            }
            catch (Exception)
            {

                return new ServiceResponse<bool>() { Data = false };
            }
            
        }

        public async Task<ServiceResponse<bool>> UpdateQuantity(Models.CartItem cartItem)
        {
            var dbCartItem = await ecommDatabaseContext.CartItems.FirstOrDefaultAsync(p => p.ProductId == cartItem.ProductId && p.ProductTypeId == cartItem.ProductTypeId && p.UserId == GetUserId());
            if(dbCartItem==null)
            {
                return new ServiceResponse<bool>()
                {
                    Data = false,
                    Success = false,
                    Message = "Cart Item does not exist",

                };
            }
            dbCartItem.Quantity = cartItem.Quantity;
            await ecommDatabaseContext.SaveChangesAsync();
            return new ServiceResponse<bool>() { Data = true };
        }

        public async Task<ServiceResponse<bool>> RemoveItemfromCart(int productId, int productTypeId)
        {
            var dbCartItem = await ecommDatabaseContext.CartItems.FirstOrDefaultAsync(p => p.ProductId == productId
                                                     && p.ProductTypeId == productTypeId && p.UserId == GetUserId());
            if (dbCartItem == null)
            {
                return new ServiceResponse<bool>()
                {
                    Data = false,
                    Success = false,
                    Message = "Cart Item does not exist",

                };
            }

            ecommDatabaseContext.Remove(dbCartItem);
            await ecommDatabaseContext.SaveChangesAsync();

            return new ServiceResponse<bool>() { Data = true };
        }
    }
}
