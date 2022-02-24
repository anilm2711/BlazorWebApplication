using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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

        [JsonIgnore]
        public virtual ICollection<ProductVariant> ProductVariants { get; set; }
    }
}
