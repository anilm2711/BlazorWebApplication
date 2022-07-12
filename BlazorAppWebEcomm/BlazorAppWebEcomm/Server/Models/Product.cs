using System;
using System.Collections.Generic;

namespace BlazorAppWebEcomm.Server.Models
{
    public partial class Product
    {
        public Product()
        {
            Images = new HashSet<Image>();
            OrderItems = new HashSet<OrderItem>();
            ProductVariants = new HashSet<ProductVariant>();
        }

        public int ProductId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public int? CategoryId { get; set; }
        public bool Featured { get; set; }
        public bool? Visible { get; set; }
        public bool Deleted { get; set; }

        public virtual Category? Category { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<ProductVariant> ProductVariants { get; set; }
    }
}
