using System;
using System.Collections.Generic;

namespace BlazorAppWebEcomm.Server.Models
{
    public partial class ProductType
    {
        public ProductType()
        {
            ProductVariants = new HashSet<ProductVariant>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<ProductVariant> ProductVariants { get; set; }
    }
}
