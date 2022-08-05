using CakeCompany.Core.Dtos;

namespace CakeCompany.Core.Interfaces;

public interface IShipmentProvider
{
    Task<List<Product>?> GetShipmentAsync();
}
