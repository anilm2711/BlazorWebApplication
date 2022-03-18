using System;
using System.Collections.Generic;

namespace BlazorAppWebEcomm.Server.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal? TotalPrice { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
