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
        public HttpClient http { get; set; }

        public List<Product> Products { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var result = await http.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product");
            if (result != null && result.Data != null)
                Products = result.Data;
        }
       

    }
}
