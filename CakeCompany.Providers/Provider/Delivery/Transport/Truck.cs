using CakeCompany.Core.Dtos;
using CakeCompany.Core.Interfaces.Transport;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CakeCompany.Providers.Provider.Delivery.Transport;

public class Truck : IDelivery<Truck>
{
    private readonly ILogger<Truck> _logger;

    public Truck(ILogger<Truck> logger)
    {
        _logger = logger;
    }
    public async Task<bool> Deliver(List<Product> products)
    {
        _logger.LogDebug($"Product(s) {JsonSerializer.Serialize(products)} has been delivered by {nameof(Truck)}");
        return await Task.FromResult(true);
    }
}