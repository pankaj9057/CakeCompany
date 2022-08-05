using CakeCompany.Core.Dtos;

namespace CakeCompany.Core.Interfaces.Transport;

public interface IVan
{
    Task<bool> Deliver(List<Product> products);
}
