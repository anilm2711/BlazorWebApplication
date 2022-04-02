using Microsoft.AspNetCore.Components;

namespace BlazorAppWebEcomm.Client.Pages.Admin
{
    public partial class ProductTypes : ComponentBase,IDisposable
    {
        [Inject]
        public IProductTypeService productTypeService { get; set; }

        ProductType editingProductType = null;
        protected override async Task OnInitializedAsync()
        {
            await productTypeService.GetProductTypes();
            productTypeService.OnChange += StateHasChanged;
        }
       private void EditProductType(ProductType productType)
        {
            productType.Editing = true;
            editingProductType = productType;
        }
        private void CreateNewProductType()
        {
           editingProductType = productTypeService.CreateNewProductType();
        }
        private async Task UpdateProductType()
        {
            if (editingProductType.IsNew == true)
            {
                await productTypeService.AddProductType(editingProductType);
            }
            else
            {
                await productTypeService.UpdateProductType(editingProductType);
            }
            editingProductType = new ProductType();
        }

        public void Dispose()
        {
            productTypeService.OnChange -= StateHasChanged;
        }
    }
}
