using CakeCompany.Core.Dtos;

namespace CakeCompany.Core.Interfaces;

public interface ITransportProvider
{
    Task<Type> CheckForAvailability(List<Product> products);
}
