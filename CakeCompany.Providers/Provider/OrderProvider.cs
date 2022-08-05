using CakeCompany.Core.Dtos;
using CakeCompany.Core.Interfaces;
using CakeCompany.Core.Enums;

namespace CakeCompany.Providers.Provider;

public class OrderProvider : IOrderProvider
{
    public async Task<Order[]> GetLatestOrders()
    {
        return await Task.FromResult(new Order[]
        {
            new("CakeBox", DateTime.Now, 1, Cake.RedVelvet, 120.25),
            new("ImportantCakeShop", DateTime.Now, 2, Cake.RedVelvet, 120.25),
            new("VanillaCake", DateTime.Now.Add(TimeSpan.FromHours(30)), 3, Cake.Vanilla, 120.25),
            new("ChocolateCake", DateTime.Now.Add(TimeSpan.FromMinutes(240)), 4, Cake.Chocolate, 120.25)
        });
    }

    public async Task UpdateOrders(Order[] orders)
    {
        await Task.FromResult<Order[]>(orders);
    }
}


