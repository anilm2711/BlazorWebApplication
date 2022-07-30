using BlazorAppWebEcomm.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppWebEcomm.Server.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly ECommDatabaseContext _eCommDataBaseContext;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ProductService(ECommDatabaseContext eCommDataBaseContext,IHttpContextAccessor httpContextAccessor)
        {
            this._eCommDataBaseContext = eCommDataBaseContext;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<Models.Product>> GetProductAsync(int productId)
        {
            var response = new ServiceResponse<Models.Product>();
            Models.Product product = null;
            if (httpContextAccessor.HttpContext.User.IsInRole("Admin") == true)
            {
                product = await _eCommDataBaseContext.Products
                 .Include(e => e.ProductVariants.Where(p => p.Visible == true))
                 .ThenInclude(x => x.ProductType)
                 .Include(e=>e.Images)
                 .FirstOrDefaultAsync(p => p.ProductId == productId && p.Visible == true);
            }
            else
            {
                product = await _eCommDataBaseContext.Products
                    .Include(e => e.ProductVariants.Where(p => p.Visible == true && p.Deleted == false))
                    .ThenInclude(x => x.ProductType)
                    .Include(e=>e.Images)
                    .FirstOrDefaultAsync(p => p.ProductId == productId && p.Visible == true && p.Deleted == false);
            }
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
                    Data = await _eCommDataBaseContext.Products.Include(x=>x.Images).Where(x=> x.Visible == true && x.Deleted == false).Include(x => x.ProductVariants.Where(v=> v.Visible == true && v.Deleted == false)).Where(p => p.Category.Url.ToLower().Equals(categoryUrl.ToLower())).ToListAsync()
                };
            }
            catch (Exception ex)
            {
                responseProdcuts.Success = false;
                responseProdcuts.Message = ex.Message;
            }
            return responseProdcuts;
        }

        public async Task<ServiceResponse<List<Models.Product>>> GetProductsAsync()
        {
            ServiceResponse<List<Models.Product>> responseProdcuts = new ServiceResponse<List<Models.Product>>();
            try
            {
                responseProdcuts = new ServiceResponse<List<Models.Product>>()
                {
                    Data = await _eCommDataBaseContext.Products.Where(p => p.Visible == true && p.Deleted == false)
                                                        .Include(e => e.Images)
                                                        .Include(e => e.ProductVariants.Where(p=>p.Visible==true && p.Deleted==false))
                                                        .ToListAsync()
                };
            }
            catch (Exception ex)
            {
                responseProdcuts.Success = false;
                responseProdcuts.Message = ex.Message;
            }
            return responseProdcuts;
        }

        public async Task<ServiceResponse<ProductSearchResult<Models.Product>>> SearchProducts(string searchText, int page)
        {
            int pageResult = 2;

            decimal r = (await FindProduct(searchText)).Count()/pageResult;
            int pageCount =Convert.ToInt32(Math.Ceiling(r));
            List<Models.Product>? products = await _eCommDataBaseContext.Products.Where(x => x.Description.ToLower().Contains(searchText.ToLower())
                                  || x.Title.ToLower().Contains(searchText.ToLower()) && x.Visible == true && x.Deleted == false)
                                  .Include(e => e.ProductVariants.Where(x=> x.Visible == true && x.Deleted == false))
                                  .Include(x=>x.Images)
                                 .Skip((page - 1) * pageResult)
                                 .Take(pageResult)
                                 .ToListAsync();
            var response = new ServiceResponse<ProductSearchResult<Models.Product>>()
            {
                Data = new ProductSearchResult<Models.Product>()
                {
                    Products = products,
                    CurrentPage = page,
                    Pages = pageCount
                }
            };
            return response;
        }
            


        public async Task<List<Models.Product>> FindProduct(string searchText)
        {
            return await _eCommDataBaseContext.Products.Where(x => x.Description.ToLower().Contains(searchText.ToLower())
                                || x.Title.ToLower().Contains(searchText.ToLower()) && x.Visible==true && x.Deleted==false)
                .Include(e => e.ProductVariants.Where(p=> p.Visible == true && p.Deleted == false))
                .Include(x=>x.Images)
                .ToListAsync();
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

        public async Task<ServiceResponse<List<Models.Product>>> GetFeaturedProductsAsync()
        {
            ServiceResponse<List<Models.Product>> responseProdcuts = new ServiceResponse<List<Models.Product>>();
            var images =  _eCommDataBaseContext.Products.Include(p=>p.Images).Where(p => p.ProductId == 1043).FirstOrDefault().Images;
            try
            {
                responseProdcuts = new ServiceResponse<List<Models.Product>>()
                {
                    Data = await _eCommDataBaseContext.Products.Include(p=>p.Images).Where(x=> x.Visible == true && x.Deleted == false)
                    .Include(e => e.ProductVariants.Where(x=> x.Visible == true && x.Deleted == false))
                    .Where(p=>p.Featured==true).ToListAsync()
                };
            }
            catch (Exception ex)
            {
                responseProdcuts.Success = false;
                responseProdcuts.Message = ex.Message;
            }
            return responseProdcuts;
        }

        public async Task<ServiceResponse<List<Models.Product>>> GetAdminProducts()
        {
            ServiceResponse<List<Models.Product>> responseProdcuts = new ServiceResponse<List<Models.Product>>();
            try
            {
                responseProdcuts = new ServiceResponse<List<Models.Product>>()
                {
                    Data = await _eCommDataBaseContext.Products.Where(p => p.Deleted == false)
                    .Include(e => e.ProductVariants.Where(p => p.Deleted == false))
                    .ThenInclude(v=>v.ProductType)
                    .Include(x=>x.Images)
                    .ToListAsync()
                    
                };
            }
            catch (Exception ex)
            {
                responseProdcuts.Success = false;
                responseProdcuts.Message = ex.Message;
            }
            return responseProdcuts;
        }

        public async Task<ServiceResponse<Models.Product>> CreateProductAsync(Models.Product product)
        {
            foreach (var item in product.ProductVariants)
            {
                item.ProductType = null;
            }
            _eCommDataBaseContext.Products.Add(product);
            await _eCommDataBaseContext.SaveChangesAsync();
            return new ServiceResponse<Models.Product>()
            {
                Data=product
            };
        }

        public async Task<ServiceResponse<Models.Product>> UpdateProductAsync(Models.Product product)
        {
            var dbProduct = await _eCommDataBaseContext.Products
                                                        .Include(p=>p.Images)
                                                        .Where(p=>p.ProductId==product.ProductId).FirstOrDefaultAsync();
            if (dbProduct == null)
            {
                return new ServiceResponse<Models.Product>()
                { 
                    Success = false,
                    Message = "Product not found."
                };
            }
            dbProduct.Title=product.Title;
            dbProduct.Description = product.Description;
            dbProduct.CategoryId = product.CategoryId;
            dbProduct.Visible = product.Visible;
            dbProduct.Title = product.Title;
            dbProduct.Featured  = product.Featured;

            //Update funactionality --  Remove all the images related to Product
            _eCommDataBaseContext.Images.RemoveRange(dbProduct.Images);

            //Update multiple Images
            dbProduct.Images = product.Images;


            foreach (var item in product.ProductVariants)
            {
                var dbVariant =await _eCommDataBaseContext.ProductVariants.Where(p => p.ProductId == item.ProductId
                  && p.ProductTypeId == item.ProductTypeId).ToListAsync();
                if (dbVariant == null || dbVariant.Count == 0)
                {
                    item.ProductType = null;
                    _eCommDataBaseContext.ProductVariants.Add(item);
                }
                else
                {
                    foreach (var x in dbVariant)
                    {
                        x.ProductTypeId = item.ProductTypeId;
                        x.Price = item.Price;
                        x.OriginalPrice = item.OriginalPrice;
                        x.Visible = item.Visible;
                        x.Deleted = item.Deleted;

                    }
                }
            }
            await _eCommDataBaseContext.SaveChangesAsync();

            return new ServiceResponse<Models.Product>
            {
                Data = product
            };
        }

        public async Task<ServiceResponse<bool>> DeleteProductAsync(int productId)
        {
            var dbProduct =await  _eCommDataBaseContext.Products.FindAsync(productId);
            if(dbProduct==null)
            {
                return new ServiceResponse<bool>() { Success = false, Data = false, Message = "Product not found." };
            }
            dbProduct.Deleted = true;
            await _eCommDataBaseContext.SaveChangesAsync();
            return new ServiceResponse<bool> { Data = true };
        }
    }
}
