using CakeCompany.Core.Dtos;

namespace CakeCompany.Core.Interfaces;

public interface IDeliveryProvider
{
    Task<bool> Deliver(List<Product> products, Type transportType);
}
