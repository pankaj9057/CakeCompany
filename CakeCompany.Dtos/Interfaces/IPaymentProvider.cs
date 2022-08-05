using CakeCompany.Core.Dtos;

namespace CakeCompany.Core.Interfaces;

public interface IPaymentProvider
{
    Task<PaymentIn> Process(Order order);
}
