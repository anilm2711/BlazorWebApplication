using System;
using System.Collections.Generic;

namespace BlazorAppWebEcomm.Server.Models
{
    public partial class ProductVariant
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int? ProductTypeId { get; set; }
        public decimal? Price { get; set; }
        public decimal? OriginalPrice { get; set; }

        public virtual Product? Product { get; set; }
        public virtual ProductType? ProductType { get; set; }
    }
}
