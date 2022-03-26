using Stripe.Checkout;

namespace BlazorAppWebEcomm.Server.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<Session> CreateCheckoutSession();
        Task<ServiceResponse<bool>> FulFillOrder(HttpRequest httpRequest);
    }
}
