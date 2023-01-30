using CakeCompany.Core.Dtos;
using CakeCompany.Core.Interfaces;
using CakeCompany.Providers.Provider;
using MediatR;

namespace CakeCompany.Providers.Handlers.Orders.Query
{
    public class TransportHandler : IRequestHandler<TransportRequest, Type>
    {
        private readonly ITransportProvider _transportProvider;
        public TransportHandler(ITransportProvider transportProvider)
        {
            _transportProvider = transportProvider;
        }
        public async Task<Type> Handle(TransportRequest request, CancellationToken cancellationToken)
        {
           return await _transportProvider.CheckForAvailability(request.products);
        }
    }
}
