using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;

namespace BlazorAppWebEcomm.Client.Shared
{
    public partial class ProductList : ComponentBase,IDisposable
    {
        [Inject]
        public IProductService ProductService { get; set; }

        protected override void OnInitialized()
        {
            ProductService.ProductsChanged += StateHasChanged;
        }
       
        public void Dispose()
        {
            ProductService.ProductsChanged-= StateHasChanged;
        }
        
    }
}
