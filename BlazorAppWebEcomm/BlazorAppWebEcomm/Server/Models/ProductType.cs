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
        public bool? Editing { get; set; }
        public bool? IsNew { get; set; }

        public virtual ICollection<ProductVariant> ProductVariants { get; set; }
    }
}
