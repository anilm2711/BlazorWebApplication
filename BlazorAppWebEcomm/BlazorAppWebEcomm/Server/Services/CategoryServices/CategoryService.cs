


namespace BlazorAppWebEcomm.Server.Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly EcommDatabaseContext _eCommDataBaseContext;

        public CategoryService(EcommDatabaseContext eCommDataBaseContext)
        {
            this._eCommDataBaseContext = eCommDataBaseContext;
        }

        public async Task<ServiceResponse<Models.Category>> GetCategoryAsync(int productId)
        {
            var response = new ServiceResponse<Models.Category>();
            var product = await _eCommDataBaseContext.Categories.FindAsync(productId);
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

        public async Task<ServiceResponse<List<Models.Category>>> GetCategoriesAsync()
        {
            ServiceResponse<List<Models.Category>> responseProdcuts = new ServiceResponse<List<Models.Category>>();
            try
            {
                responseProdcuts = new ServiceResponse<List<Models.Category>>()
                {
                    Data = await _eCommDataBaseContext.Categories.ToListAsync()
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
