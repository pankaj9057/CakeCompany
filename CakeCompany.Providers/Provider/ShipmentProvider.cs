using CakeCompany.Core.Dtos;
using CakeCompany.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using MediatR;
using CakeCompany.Providers.Handlers.Orders.Query;

namespace CakeCompany.Providers.Provider;

public class ShipmentProvider : IShipmentProvider
{ 
    private readonly IMediator _mediator;
    private readonly ILogger<ShipmentProvider> _logger;
    public ShipmentProvider(IMediator mediator, ILogger<ShipmentProvider> logger)
    {
        _mediator= mediator;
        _logger = logger;
    }

    /// <summary>
    /// This method can be used to get the shipments
    /// </summary>
    /// <returns>List of processed order, throw exception if item not delivered</returns>
    public async Task<List<Product>?> GetShipmentAsync()
    {
        //Get latest orders
        var orders = await _mediator.Send(new OrderRequest(),CancellationToken.None);
        ArgumentNullException.ThrowIfNull(orders);
        if (!orders.Any())
        {
            _logger.LogInformation("No order is available to process");
            return null;
        }

        _logger.LogInformation($"{orders.Length} order(s) has been found and getting processed");

        //Get Product by Order
        var products = await GetProductListByOrders(orders);
        //Check Transport Availability
        var transport = await _mediator.Send(new TransportRequest
        {
            products = products
        }, CancellationToken.None);


        //Deliver the product by transport
        var isDelivered = await _mediator.Send(new DeliveryRequest
        {
            productList= products,
            transportType = transport
        }, CancellationToken.None);

        if (isDelivered)
        {
            _logger.LogInformation($"{products.Count} product(s) has been shipped");
            return products;
        }
        throw new Exception("Product Delivery has been failed");
    }

    /// <summary>
    /// Get the product list by array of orders
    /// </summary>
    /// <param name="orders">Array of orders</param>
    /// <returns>List of product</returns>
    private async Task<List<Product>> GetProductListByOrders(Order[] orders)
    {
        var products = new List<Product>();

        foreach (var order in orders)
        {
            _logger.LogInformation($"Order {order.Id} is getting processed");
            var estimatedBakeTime = await _mediator.Send(new CheckCakeRequest { order = order }, CancellationToken.None);

            //Cancelled and payment failure order check
            if ((estimatedBakeTime > order.EstimatedDeliveryTime)
                ||
                !(await _mediator.Send(new PaymentRequest { order = order}, CancellationToken.None)).IsSuccessful)
            {
                _logger.LogDebug($"Excluded Order: {JsonSerializer.Serialize(order)}");
                continue;
            }

            var product = await _mediator.Send(new BakeCakeRequest { order=order}, CancellationToken.None);
            products.Add(product);
        }

        return products;
    }
}
