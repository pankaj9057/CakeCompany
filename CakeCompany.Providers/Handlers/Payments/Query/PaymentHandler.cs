using CakeCompany.Core.Dtos;
using CakeCompany.Core.Interfaces;
using CakeCompany.Providers.Provider;
using MediatR;

namespace CakeCompany.Providers.Handlers.Orders.Query
{
    public class PaymentHandler : IRequestHandler<PaymentRequest, PaymentIn>
    {
        private readonly IPaymentProvider _paymentProvider;
        public PaymentHandler(IPaymentProvider paymentProvider)
        {
            _paymentProvider = paymentProvider;
        }
        public Task<PaymentIn> Handle(PaymentRequest request, CancellationToken cancellationToken)
        {
           return _paymentProvider.Process(request.order);
        }
    }
}
