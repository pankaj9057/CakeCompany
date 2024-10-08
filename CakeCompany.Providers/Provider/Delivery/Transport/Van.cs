﻿using CakeCompany.Core.Dtos;
using CakeCompany.Core.Interfaces.Transport;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CakeCompany.Providers.Provider.Delivery.Transport;

public class Van : IDelivery<Van>
{
    private readonly ILogger<Van> _logger;

    public Van(ILogger<Van> logger)
    {
        _logger = logger;
    }
    public async Task<bool> Deliver(List<Product> products)
    {
        _logger.LogDebug($"Product(s) {JsonSerializer.Serialize(products)} has been delivered by {nameof(Van)}");
        return await Task.FromResult(true);
    }     
}