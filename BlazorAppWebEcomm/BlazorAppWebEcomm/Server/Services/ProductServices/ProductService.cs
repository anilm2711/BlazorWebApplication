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

        public async Task<ServiceResponse<List<Models.Product>>> SearchProducts(string searchText)
        {
            ServiceResponse<List<Models.Product>> responseProdcuts = new ServiceResponse<List<Models.Product>>();
            try
            {
                responseProdcuts = new ServiceResponse<List<Models.Product>>()
                {
                    Data = await FindProduct(searchText)
                };
            }
            catch (Exception ex)
            {
                responseProdcuts.Success = false;
                responseProdcuts.Message = ex.Message;
            }
            return responseProdcuts;

           
        }

        public async Task<List<Models.Product>> FindProduct(string searchText)
        {
            return await _eCommDataBaseContext.Products.Where(x => x.Description.ToLower().Contains(searchText.ToLower())
                                || x.Title.ToLower().Contains(searchText.ToLower())).Include(e => e.ProductVariants).ToListAsync();
        }

        public async Task<ServiceResponse<List<string>>> GetProductSearchSuggestion(string searchText)
        {
            var product =await FindProduct(searchText);
            List<string> prodString = new List<string>();
            foreach(var item in product)
            {
                if(item.Title.Contains(searchText,StringComparison.OrdinalIgnoreCase))
                {
                    prodString.Add(item.Title);
                }

                if(item.Description!=null)
                {
                    var punctuation = item.Description.Where(char.IsPunctuation).Distinct().ToArray();
                    var words = item.Description.Split().Select(s => s.Trim(punctuation));
                    foreach(var wrd in words)
                    {
                        if(wrd.Contains(searchText,StringComparison.OrdinalIgnoreCase) && prodString.Contains(wrd)==false)
                        {
                            prodString.Add(wrd);
                        }
                    }
                }
            }

            return new ServiceResponse<List<string>> { Data = prodString };
        }
    }
}
