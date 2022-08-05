using CakeCompany.Core.Dtos;
using CakeCompany.Core.Interfaces;
using MediatR;

namespace CakeCompany.Application.Shipment;

public class ShipmentHandler : IRequestHandler<ShipmentRequest, List<Product>?>
{
    private readonly IShipmentProvider _shipmentProvider;

    public ShipmentHandler(IShipmentProvider shipmentProvider)
    {
        _shipmentProvider = shipmentProvider;
    }
    public async Task<List<Product>?> Handle(ShipmentRequest request, CancellationToken cancellationToken)
    {
        return await _shipmentProvider.GetShipmentAsync();
    }
}
