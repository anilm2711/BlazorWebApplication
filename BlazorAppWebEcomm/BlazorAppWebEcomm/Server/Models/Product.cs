using System;
using System.Collections.Generic;

namespace BlazorAppWebEcomm.Server.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderItems = new HashSet<OrderItem>();
            ProductVariants = new HashSet<ProductVariant>();
        }

        public int ProductId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public int? CategoryId { get; set; }
        public bool? Featured { get; set; }

        public virtual Category? Category { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<ProductVariant> ProductVariants { get; set; }
    }
}
