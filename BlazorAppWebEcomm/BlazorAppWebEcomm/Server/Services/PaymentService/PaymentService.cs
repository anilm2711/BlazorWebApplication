using Stripe;
using Stripe.Checkout;

namespace BlazorAppWebEcomm.Server.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly ICartService cartService;
        private readonly IAuthService authService;
        private readonly IOrderService orderService;
        private const string secretkey = "whsec_1d811f41644a52f29c7601fe3a3fbc7c829c7af995455716aa1a096b8ec7c59b";

        public PaymentService(ICartService cartService,IAuthService authService,IOrderService orderService)
        {
            StripeConfiguration.ApiKey = "sk_test_51KfmwUSAmkpQijNHymjLVUzjEWDrwM33Cv1NG0NBKuYwqtGIOAsRUC0IlLoQTP4Xm7sqlYzP1NlZEHVWP8euakBY00Q2rlSCDi";
            this.cartService = cartService;
            this.authService = authService;
            this.orderService = orderService;
        }
        public async Task<Session> CreateCheckoutSession()
        {
            var products = (await cartService.GetDbCartProducts()).Data;
            var lineItems = new List<SessionLineItemOptions>();
            products.ForEach(product => lineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmountDecimal = product.Price * 100,
                    Currency="INR",
                    ProductData=new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = product.Title,
                        Images = new List<string> { product.ImageUrl }
                    }
                },
                Quantity=product.Quantity 

            }));
            var options = new SessionCreateOptions
            {
                CustomerEmail = authService.GetUserEmail(),
                PaymentMethodTypes=new List<string>
                {
                    "card"
                },
                ShippingAddressCollection=new SessionShippingAddressCollectionOptions
                {
                    AllowedCountries=new List<string>
                    {
                        "IN"
                    }
                },
                LineItems=lineItems,
                Mode="payment",
                SuccessUrl= "https://localhost:44362/order-success",
                CancelUrl= "https://localhost:44362/cart"

            };
            var service = new SessionService();
            Session session = service.Create(options);
            return session;

        }

        public async Task<ServiceResponse<bool>> FulFillOrder(HttpRequest httpRequest)
        {
            var jason = await new StreamReader(httpRequest.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent( jason,httpRequest.Headers["Stripe-Signature"],secretkey);
                if(stripeEvent.Type==Events.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Session;
                    var user =await authService.GetUserByEmailId(session.CustomerEmail);
                    await orderService.PlaceOrder(user.Id);
                }
                return new ServiceResponse<bool> { Data = true };
            }
            catch (StripeException e)
            {

                return new ServiceResponse<bool> { Data = false, Success = false, Message = e.Message };
            }
        }
    }
}
