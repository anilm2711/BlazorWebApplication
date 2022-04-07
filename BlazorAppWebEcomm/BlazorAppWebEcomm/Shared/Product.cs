using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAppWebEcomm.Shared
{
    public class Product
    {
        public int ProductId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public bool Featured { get; set; }
        public Category Category { get; set; }
        public int? CategoryId { get; set; }
        public List<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();

        public bool Visible { get; set; } = true;
        public bool Deleted { get; set; } = false;
        public bool? Editing { get; set; }
        public bool? IsNew { get; set; }

    }
}
