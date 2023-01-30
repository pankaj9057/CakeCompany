using CakeCompany.Core.Dtos;
using MediatR;

namespace CakeCompany.Providers.Handlers.Orders.Query
{
    public class PaymentRequest : IRequest<PaymentIn>
    {
        public Order order { get; set; }

    }
}
