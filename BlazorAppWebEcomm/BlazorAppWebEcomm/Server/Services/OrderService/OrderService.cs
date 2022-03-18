using System.Security.Claims;

namespace BlazorAppWebEcomm.Server.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly EcommDatabaseContext context;
        private readonly ICartService cartService;
        private readonly IAuthService authService;

        public OrderService(EcommDatabaseContext context, ICartService cartService, IAuthService authService)
        {
            this.context = context;
            this.cartService = cartService;
            this.authService = authService;
        }

        public async Task<ServiceResponse<List<OrderOverViewResponse>>> GetOrders()
        {
            var response = new ServiceResponse<List<OrderOverViewResponse>>();
            try
            {
                var OrderOverViewResponseList = new List<OrderOverViewResponse>();
                var orders =await  context.Orders.Include(p => p.OrderItems)
                                        .ThenInclude(p => p.Product)
                                        .Where(p => p.UserId == authService.GetUserId())
                                        .OrderByDescending(p => p.OrderDate).ToListAsync();

                orders.ForEach(x => OrderOverViewResponseList.Add(new OrderOverViewResponse
                {
                    Id = x.Id,
                    OrderDate=x.OrderDate,
                    TotalPrice=(decimal) x.TotalPrice,
                    Product = x.OrderItems.Count() > 1 ?$"{x.OrderItems.FirstOrDefault().Product.Title} and "
                                +$"{x.OrderItems.Count-1} more..":x.OrderItems.FirstOrDefault().Product.Title,
                    ProductImageUrl=x.OrderItems.FirstOrDefault().Product.ImageUrl
                })) ;
                response.Data = OrderOverViewResponseList.OrderByDescending(p => p.OrderDate).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
            return response;
        }

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
                UserId = authService.GetUserId(),
                OrderDate = DateTime.Now,
                TotalPrice = totalPrice,
                OrderItems = orderItems
            };

            context.Orders.Add(order);
            context.CartItems.RemoveRange(context.CartItems.Where(p => p.UserId == authService.GetUserId()));
            await context.SaveChangesAsync();
            return new ServiceResponse<bool>() { Data = true };
        }
    }
}
