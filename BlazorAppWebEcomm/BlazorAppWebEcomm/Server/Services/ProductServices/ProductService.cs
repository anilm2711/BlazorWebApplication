using BlazorAppWebEcomm.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppWebEcomm.Server.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly ECommDatabaseContext _eCommDataBaseContext;

        public ProductService(ECommDatabaseContext eCommDataBaseContext)
        {
            this._eCommDataBaseContext = eCommDataBaseContext;
        }

        public async Task<ServiceResponse<Models.Product>> GetProductAsync(int productId)
        {
            var response = new ServiceResponse<Models.Product>();
            var product =await _eCommDataBaseContext.Products
                .Include(e=>e.ProductVariants)
                .ThenInclude(x=>x.ProductType).FirstOrDefaultAsync(p => p.ProductId == productId);
            if (product == null)
            {
                response.Success = false;
                response.Message = "Sorry, Product you are looking is not avaialble.";
            }
            else
            {
                response.Data = product;
            }
            return response;
        }

        public async Task<ServiceResponse<List<Models.Product>>> GetProductByCategoryAsync(string categoryUrl)
        {
            ServiceResponse<List<Models.Product>> responseProdcuts = new ServiceResponse<List<Models.Product>>();
            try
            {
                responseProdcuts = new ServiceResponse<List<Models.Product>>()
                {
                    Data = await _eCommDataBaseContext.Products.Include(x=>x.ProductVariants).Where(p => p.Category.Url.ToLower().Equals(categoryUrl.ToLower())).ToListAsync()
                };
            }
            catch (Exception ex)
            {
                responseProdcuts.Success = false;
                responseProdcuts.Message = ex.Message;
            }
            return responseProdcuts;
        }

        public  async Task<ServiceResponse<List<Models.Product>>> GetProductsAsync()
        {
            ServiceResponse<List<Models.Product>> responseProdcuts = new ServiceResponse<List<Models.Product>>();
            try
            {
                responseProdcuts = new ServiceResponse<List<Models.Product>>()
                {
                    Data = await _eCommDataBaseContext.Products.Include(e=>e.ProductVariants).ToListAsync()
                };
            }
            catch (Exception ex)
            {
                responseProdcuts.Success = false;
                responseProdcuts.Message = ex.Message;
            }
            return responseProdcuts;
        }
    }
}
