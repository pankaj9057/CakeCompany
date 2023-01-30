using CakeCompany.Core.Dtos;
using CakeCompany.Core.Interfaces;
using MediatR;

public class DeliveryHandler : IRequestHandler<DeliveryRequest, bool>
{
    private readonly IDeliveryProvider _deliveryProvider;

    public DeliveryHandler(IDeliveryProvider deliveryProvider)
    {
        _deliveryProvider = deliveryProvider;
    }
    public async Task<bool> Handle(DeliveryRequest request, CancellationToken cancellationToken)
    {
        return await _deliveryProvider.Deliver(request.productList,request.transportType);
    }
}