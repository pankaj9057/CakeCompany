﻿using CakeCompany.Core.Dtos;
using CakeCompany.Core.Interfaces;
using CakeCompany.Providers.Provider.Delivery.Transport;

namespace CakeCompany.Providers.Provider;

public class TransportProvider : ITransportProvider
{

    /// <summary>
    /// Check th available transport as per product quantity
    /// </summary>
    /// <param name="products">List of product</param>
    /// <returns>Available transport name</returns>
    public async Task<Type> CheckForAvailability(List<Product> products)
    {
        if (products.Sum(p => p.Quantity) < 1000)
        {
            return await Task.FromResult(typeof(Van));
        }

        if (products.Sum(p => p.Quantity) > 1000
            && products.Sum(p => p.Quantity) < 5000)
        {
            return await Task.FromResult(typeof(Truck));
        }

        return await Task.FromResult(typeof(Ship));
    }
}
