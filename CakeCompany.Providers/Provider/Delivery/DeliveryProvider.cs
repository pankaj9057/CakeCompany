using CakeCompany.Core.Dtos;
using CakeCompany.Core.Interfaces;
using CakeCompany.Core.Interfaces.Transport;
using CakeCompany.Providers.Provider.Delivery.Transport;

namespace CakeCompany.Providers.Provider.Delivery;

public class DeliveryProvider : IDeliveryProvider
{
    private readonly IDelivery<Van> _van;
    private readonly IDelivery<Truck> _truck;
    private readonly IDelivery<Ship> _ship;

    public DeliveryProvider(IDelivery<Van> van,IDelivery<Truck> truck,IDelivery<Ship> ship)
    {
        _van = van;
        _truck = truck;
        _ship = ship;
    }
    public async Task<bool> Deliver(List<Product> products, Type transportType)
    {
        if (transportType == typeof(Van))
        {
            return await _van.Deliver(products);
        }
        if (transportType == typeof(Ship))
        {
            return await _ship.Deliver(products);
        }

        if (transportType == typeof(Truck))
        {
            return await _truck.Deliver(products);
        }
        return await Task.FromResult(false);
    }
     
}
