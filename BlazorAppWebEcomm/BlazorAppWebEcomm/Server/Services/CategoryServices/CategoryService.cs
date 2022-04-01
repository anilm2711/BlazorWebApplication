


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
                    Data = await _eCommDataBaseContext.Categories.Where(p=>p.Deleted==false && p.Visible==true).ToListAsync()
                };
            }
            catch (Exception ex)
            {
                responseProdcuts.Success = false;
                responseProdcuts.Message = ex.Message;
            }
            return responseProdcuts;
        }

        public async Task<ServiceResponse<List<Models.Category>>> GetAdminCategories()
        {
            ServiceResponse<List<Models.Category>> responseProdcuts = new ServiceResponse<List<Models.Category>>();
            try
            {
                responseProdcuts = new ServiceResponse<List<Models.Category>>()
                {
                    Data = await _eCommDataBaseContext.Categories.Where(p => p.Deleted == false).ToListAsync()
                };
            }
            catch (Exception ex)
            {
                responseProdcuts.Success = false;
                responseProdcuts.Message = ex.Message;
            }
            return responseProdcuts;
        }

        public async Task<ServiceResponse<List<Models.Category>>> AddCategory(Models.Category category)
        {
            category.Editing = category.IsNew = false;
            _eCommDataBaseContext.Categories.Add(category);
            await _eCommDataBaseContext.SaveChangesAsync();
            return await GetAdminCategories();
        }

        public async Task<ServiceResponse<List<Models.Category>>> UpdateCategory(Models.Category category)
        {
            var dbCategory = await GetCategoryById(category.Id);
            if (dbCategory == null)
            {
                return new ServiceResponse<List<Models.Category>>
                {
                    Success = false,
                    Message = "Category not found."
                };
            }
            dbCategory.Name = category.Name;
            dbCategory.Url = category.Url;
            dbCategory.Deleted = category.Deleted;
            dbCategory.Visible = category.Visible;
            await _eCommDataBaseContext.SaveChangesAsync();
            return await GetAdminCategories();
        }

        public async Task<ServiceResponse<List<Models.Category>>> DeleteCategory(int Id)
        {
            var category=await GetCategoryById(Id);
            if(category==null)
            {
                return new ServiceResponse<List<Models.Category>>
                {
                    Success = false,
                    Message = "Category not Found."
                };
            }
            category.Deleted = true;
            await _eCommDataBaseContext.SaveChangesAsync();
            return await GetAdminCategories();
        }

        private async Task<Models.Category> GetCategoryById(int Id)
        {
            return await _eCommDataBaseContext.Categories.FindAsync(Id);
        }
    }
}
