using CakeCompany.Core.Dtos;

namespace CakeCompany.Core.Interfaces;

public interface ICakeProvider
{
    Task<DateTime> Check(Order order);
    Task<Product> Bake(Order order);
}
