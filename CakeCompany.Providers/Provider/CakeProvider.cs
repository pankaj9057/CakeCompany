using CakeCompany.Core.Dtos;
using CakeCompany.Core.Interfaces;
using CakeCompany.Core.Enums;

namespace CakeCompany.Providers.Provider;

public class CakeProvider : ICakeProvider
{
    private readonly IDateTimeProvider _dateTimeProvider;
    public CakeProvider(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    /// <summary>
    /// Check and return time taken for order by Name
    /// </summary>
    /// <param name="order">Order details</param>
    /// <returns>Date time taken for processing the order</returns>
    public async Task<DateTime> Check(Order order)
    {
        if (order.Name == Cake.Chocolate)
        {
            return await Task.FromResult(_dateTimeProvider.Now().Add(TimeSpan.FromMinutes(30)));
        }

        if (order.Name == Cake.RedVelvet)
        {
            return await Task.FromResult(_dateTimeProvider.Now().Add(TimeSpan.FromMinutes(60)));
        }

        return await Task.FromResult(_dateTimeProvider.Now().Add(TimeSpan.FromHours(15)));
    }

    /// <summary>
    /// Bake the order and return the product
    /// </summary>
    /// <param name="order">Order details</param>
    /// <returns>Product details</returns>
    public async Task<Product> Bake(Order order)
    {
        if (order.Name == Cake.Chocolate)
        {
            return await Task.FromResult(GetProductByOrder(order, Cake.Chocolate));
        }

        if (order.Name == Cake.RedVelvet)
        {
            return await Task.FromResult(GetProductByOrder(order, Cake.RedVelvet));
        }

        return await Task.FromResult(GetProductByOrder(order, Cake.Vanilla));
    }

    /// <summary>
    /// Get New Product by Order and Cake type
    /// </summary>
    /// <param name="order">Order details</param>
    /// <param name="cake">Cake Type</param>
    /// <returns>New Product</returns>
    private static Product GetProductByOrder(Order order, Cake cake)
    {
        return new Product
        {
            Cake = cake,
            Id = Guid.NewGuid(),
            Quantity = order.Quantity,
            OrderId = order.Id
        };
    }
}