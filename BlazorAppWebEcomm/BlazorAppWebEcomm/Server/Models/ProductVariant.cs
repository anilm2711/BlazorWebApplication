using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BlazorAppWebEcomm.Server.Models
{
    public partial class ProductVariant
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int? ProductTypeId { get; set; }
        public decimal? Price { get; set; }
        public decimal? OriginalPrice { get; set; }
        public bool? Visible { get; set; }
        public bool Deleted { get; set; }
        [JsonIgnore]
        public virtual Product? Product { get; set; }
        public virtual ProductType? ProductType { get; set; }
    }
}
