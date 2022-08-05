using CakeCompany.Core.Dtos;

namespace CakeCompany.Core.Interfaces.Transport;

public interface IShip
{
    Task<bool> Deliver(List<Product> products);
}

