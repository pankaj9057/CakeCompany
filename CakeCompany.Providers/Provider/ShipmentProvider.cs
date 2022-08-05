using CakeCompany.Core.Dtos;
using CakeCompany.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CakeCompany.Providers.Provider;

public class ShipmentProvider : IShipmentProvider
{
    private readonly IOrderProvider _orderProvider;
    private readonly ICakeProvider _cakeProvider;
    private readonly IPaymentProvider _paymentProvider;
    private readonly ITransportProvider _transportProvider;
    private readonly IDeliveryProvider _deliveryProvider;
    private readonly ILogger<ShipmentProvider> _logger;
    public ShipmentProvider(IOrderProvider orderProvider, ICakeProvider cakeProvider,
        IPaymentProvider paymentProvider, ITransportProvider transportProvider,
        IDeliveryProvider deliveryProvider, ILogger<ShipmentProvider> logger)
    {
        _orderProvider = orderProvider;
        _cakeProvider = cakeProvider;
        _paymentProvider = paymentProvider;
        _transportProvider = transportProvider;
        _deliveryProvider = deliveryProvider;
        _logger = logger;
    }

    /// <summary>
    /// This method can be used to get the shipments
    /// </summary>
    /// <returns>List of processed order, throw exception if item not delivered</returns>
    public async Task<List<Product>?> GetShipmentAsync()
    {
        //Get latest orders
        var orders = await _orderProvider.GetLatestOrders();
        ArgumentNullException.ThrowIfNull(orders);
        if (!orders.Any())
        {
            return null;
        }

        //Get Product by Order
        var products = await GetProductListByOrders(orders);
        //Check Transport Availability
        var transport = await _transportProvider.CheckForAvailability(products);


        //Deliver the product by transport
        var isDelivered = await _deliveryProvider.Deliver(products, transport);

        if (isDelivered)
            return products;
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
            var estimatedBakeTime = await _cakeProvider.Check(order);

            //Cancelled and payment failure order check
            if ((estimatedBakeTime > order.EstimatedDeliveryTime)
                ||
                !(await _paymentProvider.Process(order)).IsSuccessful)
            {
                _logger.LogDebug($"Excluded Order: {JsonSerializer.Serialize(order)}");
                continue;
            }

            var product = await _cakeProvider.Bake(order);
            products.Add(product);
        }

        return products;
    }
}
