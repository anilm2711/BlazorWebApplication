using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAppWebEcomm.Shared
{
    public  class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public bool Visible { get; set; } = true;
        public bool Deleted { get; set; } = false;
        public bool? Editing { get; set; }
        public bool?IsNew { get; set; }
    }
}
