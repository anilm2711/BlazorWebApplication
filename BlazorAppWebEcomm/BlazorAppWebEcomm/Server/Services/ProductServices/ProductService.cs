﻿using BlazorAppWebEcomm.Server.Models;
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
            var product = await _eCommDataBaseContext.Products
                .Include(e => e.ProductVariants.Where(p => p.Visible == true && p.Deleted == false))
                .ThenInclude(x => x.ProductType).FirstOrDefaultAsync(p => p.ProductId == productId && p.Visible == true && p.Deleted == false) ;
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
                    Data = await _eCommDataBaseContext.Products.Where(x=> x.Visible == true && x.Deleted == false).Include(x => x.ProductVariants.Where(v=> v.Visible == true && v.Deleted == false)).Where(p => p.Category.Url.ToLower().Equals(categoryUrl.ToLower())).ToListAsync()
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
                    .Include(e => e.ProductVariants.Where(p=>p.Visible==true && p.Deleted==false)).ToListAsync()
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
                                  || x.Title.ToLower().Contains(searchText.ToLower()) && x.Visible == true && x.Deleted == false).Include(e => e.ProductVariants.Where(x=> x.Visible == true && x.Deleted == false))
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
                                || x.Title.ToLower().Contains(searchText.ToLower()) && x.Visible==true && x.Deleted==false).Include(e => e.ProductVariants.Where(p=> p.Visible == true && p.Deleted == false)).ToListAsync();
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
            try
            {
                responseProdcuts = new ServiceResponse<List<Models.Product>>()
                {
                    Data = await _eCommDataBaseContext.Products.Where(x=> x.Visible == true && x.Deleted == false).Include(e => e.ProductVariants.Where(x=> x.Visible == true && x.Deleted == false)).Where(p=>p.Featured==true).ToListAsync()
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
                    .Include(e => e.ProductVariants.Where(p => p.Deleted == false)).ThenInclude(v=>v.ProductType).ToListAsync()
                    
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
