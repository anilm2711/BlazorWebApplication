using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorAppWebEcomm.Client.Pages
{
    public partial class ProductDetail : ComponentBase
    {
        [Inject]
        public IProductService  ProductService { get; set; }
        private Product Product { get; set; } = null;
        public string message { get; set; } = string.Empty;

        private int ?currentTypeId { get; set; } = 1;

        [Parameter]
        public int Id { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            message = "Loading Product..";
            var response = await ProductService.GetProduct(Id);
            if (response.Success == false)
            {
                message = response.Message;
            }
            else
            {
                Product = response.Data;
                if(Product.ProductVariants!=null && Product.ProductVariants.Count()>0)
                {
                    currentTypeId = Product.ProductVariants.FirstOrDefault().ProductTypeId;
                }
            }
        }
        internal ProductVariant GetSelectedProductVariant()
        {
            var pVariant = Product.ProductVariants.FirstOrDefault(p => p.ProductTypeId == currentTypeId);
            return pVariant;
        }
    }
}
