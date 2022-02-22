using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;

namespace BlazorAppWebEcomm.Client.Shared
{
    public partial class ProductList : ComponentBase
    {

        [Inject]
        public IProductService ProductService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await ProductService.GetProducts();
        }
       

    }
}
