using System;
using System.Collections.Generic;

namespace BlazorAppWebEcomm.Server.Models
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Url { get; set; } = null!;
        public bool? Visible { get; set; }
        public bool Deleted { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
