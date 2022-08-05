
using CakeCompany.Core.Dtos;

namespace CakeCompany.Core.Interfaces.Transport;

public interface ITruck
{
    Task<bool> Deliver(List<Product> products);
}
