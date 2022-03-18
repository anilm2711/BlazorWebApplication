using System;
using System.Collections.Generic;

namespace BlazorAppWebEcomm.Server.Models
{
    public partial class OrderItem
    {
        public int OrderItemId { get; set; }
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ProductTypeId { get; set; }
        public int Quantity { get; set; }
        public decimal? TotalPrice { get; set; }

        public virtual Order IdNavigation { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
