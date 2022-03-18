using System.Security.Claims;

namespace BlazorAppWebEcomm.Server.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly EcommDatabaseContext context;
        private readonly ICartService cartService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IHttpContextAccessor httpContext;

        public OrderService(EcommDatabaseContext context, ICartService cartService,IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.cartService = cartService;
            this.httpContextAccessor = httpContextAccessor;
        }

        private int GetUserId() => int.Parse(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        public async Task<ServiceResponse<bool>> PlaceOrder()
        {
            var products = (await cartService.GetDbCartProducts()).Data;
            decimal totalPrice = 0;
            foreach (var p in products)
            {
                totalPrice += Convert.ToInt32(p.Quantity) * p.Price.Value;
            }

            var orderItems = new List<OrderItem>();
            foreach (var item in products)
            {
                OrderItem orderitem = new OrderItem();
                orderitem.ProductId = item.ProductId;
                orderitem.ProductTypeId = item.ProductTypeId;
                orderitem.Quantity = item.Quantity;
                orderitem.TotalPrice = item.Quantity * item.Price.Value;
                orderItems.Add(orderitem);
            }

            var order = new Order()
            {
                UserId = GetUserId(),
                OrderDate = DateTime.Now,
                TotalPrice = totalPrice,
                OrderItems = orderItems
            };

            context.Orders.Add(order);
            context.CartItems.RemoveRange(context.CartItems.Where(p => p.UserId == GetUserId()));
            await context.SaveChangesAsync();
            return new ServiceResponse<bool>() { Data = true };
        }
    }
}
