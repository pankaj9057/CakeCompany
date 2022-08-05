using CakeCompany.Core.Dtos;
using CakeCompany.Core.Interfaces.Transport;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CakeCompany.Providers.Provider.Delivery.Transport;

public class Ship : IShip
{
    private readonly ILogger<Ship> _logger;

    public Ship(ILogger<Ship> logger)
    {
        _logger = logger;
    }
    public async Task<bool> Deliver(List<Product> products)
    {
        _logger.LogDebug($"Product(s) {JsonSerializer.Serialize(products)} has been delivered by {nameof(Ship)}");
        return await Task.FromResult(true);
    }
}