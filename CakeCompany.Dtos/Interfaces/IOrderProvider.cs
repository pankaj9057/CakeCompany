using CakeCompany.Core.Dtos;

namespace CakeCompany.Core.Interfaces;

public interface IOrderProvider
{
    Task<Order[]> GetLatestOrders();
    Task UpdateOrders(Order[] orders);
}
