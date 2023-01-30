using CakeCompany.Core.Dtos;

namespace CakeCompany.Core.Interfaces.Transport;

public interface IDelivery<T> where T : class
{
    Task<bool> Deliver(List<Product> products);
}
