using CakeCompany.Core.Dtos;

namespace CakeCompany.Core.Interfaces;

public interface ITransportProvider
{
    Task<string> CheckForAvailability(List<Product> products);
}
