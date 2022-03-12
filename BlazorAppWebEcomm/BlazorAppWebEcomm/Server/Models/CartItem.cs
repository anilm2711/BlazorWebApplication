using System;
using System.Collections.Generic;

namespace BlazorAppWebEcomm.Server.Models
{
    public partial class CartItem
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int ProductTypeId { get; set; }
        public int Quantity { get; set; }
    }
}
