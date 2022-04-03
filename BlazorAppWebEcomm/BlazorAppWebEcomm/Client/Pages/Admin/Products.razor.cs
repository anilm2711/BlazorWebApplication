using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorAppWebEcomm.Client.Pages.Admin
{
    public partial class Products : ComponentBase
    {
        [Inject]
        public IProductService productService  { get; set; }
        [Inject]
        public NavigationManager navigationManager { get; set; }   

        protected override async Task OnInitializedAsync()
        {
            await productService.GetAdminProducts();
        }
        void EditProduct(int productId)
        {
            navigationManager.NavigateTo($"admin/product/{productId}");
        }
        void CreateProduct()
        {
            navigationManager.NavigateTo("admin/product");
        }
    }
}
