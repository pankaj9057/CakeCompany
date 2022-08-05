using CakeCompany.Core.Dtos;
using CakeCompany.Core.Interfaces;
using CakeCompany.Core.Interfaces.Transport;
using CakeCompany.Providers.Provider.Delivery.Transport;

namespace CakeCompany.Providers.Provider.Delivery;

public class DeliveryProvider : IDeliveryProvider
{
    private readonly IVan _van;
    private readonly IShip _ship;
    private readonly ITruck _truck;

    public DeliveryProvider(IVan van, IShip ship, ITruck truck)
    {
        _van = van;
        _ship = ship;
        _truck = truck;
    }
    public async Task<bool> Deliver(List<Product> products, string transportType)
    {
        if (transportType == nameof(Van))
        {
            return await _van.Deliver(products);
        }

        if (transportType == nameof(Truck))
        {
            return await _truck.Deliver(products);
        }

        if (transportType == nameof(Ship))
        {
            return await _ship.Deliver(products);
        }
        return false;
    }
}
