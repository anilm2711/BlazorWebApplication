using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorAppWebEcomm.Client.Pages.Admin
{
    public partial class EditProduct:ComponentBase
    {
        [Inject]
        public IProductService productService { get; set; }
        [Inject]
        public IProductTypeService productTypeService { get; set; }
        [Inject]
        public ICategoryService categoryService { get; set; }
        [Inject]
        public NavigationManager navigationManager { get; set; }
        [Inject]
        public IJSRuntime jSRuntime { get; set; }

        [Parameter]
        public int id { get; set; }
        Product product = new Product();
        string btnText = "";
        string message="Loading...";
        bool loading = true;

        protected override async Task OnInitializedAsync()
        {
            await productTypeService.GetProductTypes();
            await categoryService.GetAdminCategories();
        }
        protected override async Task OnParametersSetAsync()
        {
            if(id==0)
            {
                product = new Product() { IsNew = true };
                btnText = "Create Product";
            }
            else
            {
                Product dbProduct = (await productService.GetProduct(id)).Data;
                if(product ==null)
                {
                    message = $"Product with Id '{ id }' does not exist.";
                    return;
                }
                product = dbProduct;
                product.Editing = true;
                btnText = "Update Product";
            }
            loading = false;

        }
        void RemoveVariant(int productTypeId)
        {
            var Xvariant = product.ProductVariants.Where(p => p.ProductTypeId == productTypeId).ToList();
            foreach (var variant in Xvariant)
            {
                if (variant == null)
                {
                    return;
                }
                if (variant.IsNew == true)
                {
                    product.ProductVariants.Remove(variant);
                }
                else
                {
                    variant.Deleted = true;
                }
            }
        }

        void AddVariant()
        {
            product.ProductVariants.Add(new ProductVariant { IsNew=true,ProductId=product.ProductId});
        }
        async void  AddOrUpdateProduct()
        {
            if(product.IsNew==true)
            {
                var result = await productService.CreateProductAsync(product);
                navigationManager.NavigateTo($"admin/product/{result.ProductId}");
            }
            else
            {
                product.IsNew = false;
                product = await productService.UpdateProductAsync(product);
                navigationManager.NavigateTo($"admin/product/{product.ProductId}",true);
            }
        }

        async void DeleteProduct()
        {
            if (product.ProductId > 0)
            {
                var confirmed = await jSRuntime.InvokeAsync<bool>("confirm", $"do you really want to delete '{product.Title}'?");
                if (confirmed)
                {
                    await productService.DeleteProductAsync(product);
                    navigationManager.NavigateTo("admin/products");
                }
            }
        }
    }
}
