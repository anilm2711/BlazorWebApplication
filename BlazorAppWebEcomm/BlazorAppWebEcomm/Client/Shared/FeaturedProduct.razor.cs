using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorAppWebEcomm.Client.Shared
{
    public partial class FeaturedProduct : ComponentBase,IDisposable
    {
        [Inject]
        public IProductService _productService { get; set; }

        protected override void OnInitialized()
        {
            _productService.ProductsChanged += StateHasChanged;
        }
        public void Dispose()
        {
            _productService.ProductsChanged -= StateHasChanged;
        }
    }
}
