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

        public async Task<ServiceResponse<List<OrderDetailsResponse>>> GetOrderDetails(int orderId)
        {
            var response = new ServiceResponse<List<OrderDetailsResponse>>();
            List<OrderDetailsResponse> orderDetails = new List<OrderDetailsResponse>();
            List<OrderDetailProductResponse> orderDetailsResponses = new List<OrderDetailProductResponse>();
            try
            {
                var order = await context.Orders
                                       .Include(p => p.OrderItems)
                                       .ThenInclude(p => p.Product)
                                        .Where(x => x.UserId == authService.GetUserId() && x.Id == orderId)
                                        .OrderByDescending(p => p.OrderDate).ToListAsync();
                if (order == null)
                {
                    return new ServiceResponse<List<OrderDetailsResponse>>
                    {
                        Success = false,
                        Message = "Order not found."
                    };
                }

                List<int> productTypeIds = new List<int>();
                foreach (var item in order)
                {
                    OrderDetailsResponse ordResp = new OrderDetailsResponse();
                    ordResp.OrderDate = item.OrderDate;
                    ordResp.TotalPrice = Convert.ToDecimal(item.TotalPrice);
                    ordResp.Products = new List<OrderDetailProductResponse>();
                    foreach (OrderItem oItem in item.OrderItems)
                    {
                        productTypeIds.Add(oItem.ProductTypeId);
                        OrderDetailProductResponse orderDetailProduct = new OrderDetailProductResponse();
                        orderDetailProduct.ProductTypeId = oItem.ProductTypeId;
                        orderDetailProduct.ProductId = oItem.ProductId;
                        orderDetailProduct.ProductImageUrl = oItem.Product.ImageUrl;
                        orderDetailProduct.Title = oItem.Product.Title;
                        orderDetailProduct.Quantity = oItem.Quantity;
                        orderDetailProduct.TotalPrice = Convert.ToDecimal(oItem.TotalPrice);
                        orderDetailsResponses.Add(orderDetailProduct);
                    }
                    ordResp.Products.AddRange(orderDetailsResponses);
                    orderDetails.Add(ordResp);
                }
                var prodTyps = await context.ProductTypes.Where(p => productTypeIds.Contains(p.Id)).ToListAsync();
                foreach (OrderDetailProductResponse orp in orderDetailsResponses)
                {
                    orp.ProductType = prodTyps.FirstOrDefault(p => p.Id == orp.ProductTypeId).Name;
                }
                response.Data = orderDetails;
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<OrderDetailsResponse>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
            return response;
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
