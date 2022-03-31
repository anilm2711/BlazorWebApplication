namespace BlazorAppWebEcomm.Client.Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;
        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public List<Category> Categories { get; set; }
        public List<Category> AdminCategories { get ; set ; }=new List<Category>();

        public event Action OnChange;

        public async Task AddCategory(Category category)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/Category/addcategory", category);
            AdminCategories = (await result.Content.ReadFromJsonAsync<ServiceResponse<List<Category>>>()).Data;
            await GetCategories();
            OnChange.Invoke();
        }

        public Category CreateNewCategory()
        {
            var newCategory= new Category { Editing = true, IsNew = true };
            AdminCategories.Add(newCategory);
            OnChange.Invoke();
            return newCategory;
        }

        public async Task DeleteCategory(int Id)
        {
            var res = await _httpClient.DeleteAsync($"api/Category/deletecategory/{Id}");
            AdminCategories = (await res.Content.ReadFromJsonAsync<ServiceResponse<List<Category>>>()).Data;
            await GetCategories();
            OnChange.Invoke();
        }

        public async Task<List<Category>> GetAdminCategories()
        {
            var result = await _httpClient.GetFromJsonAsync<ServiceResponse<List<Category>>>("api/Category/getadmincategories");
            if (result != null && result.Data != null)
            {
                AdminCategories = result.Data;
            }
            return AdminCategories;
        }

        public async Task GetCategories()
        {
            var result =await  _httpClient.GetFromJsonAsync<ServiceResponse<List<Category>>>("api/Category");
            if(result!=null && result.Data!=null)
            {
                Categories = result.Data;
            }
        }

        public Task<ServiceResponse<Category>> GetProduct(int categoryId)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateCategory(Category category)
        {
            var result = await _httpClient.PutAsJsonAsync($"api/Category/updatecategory",category);
            AdminCategories = (await result.Content.ReadFromJsonAsync<ServiceResponse<List<Category>>>()).Data;
            await GetCategories();
            OnChange.Invoke();
        }

    }
}
