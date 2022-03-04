using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAppWebEcomm.Shared
{
    public class ProductSearchResult<T>
    {
        public List<T?> Products { get; set; } = new List<T?>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}
