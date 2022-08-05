using CakeCompany.Core.Dtos;
using CakeCompany.Core.Interfaces;

namespace CakeCompany.Providers.Provider;

public class PaymentProvider : IPaymentProvider
{
    public async Task<PaymentIn> Process(Order order)
    {
        if (order.ClientName.Contains("Important"))
        {
            return await Task.FromResult(new PaymentIn
            {
                HasCreditLimit = false,
                IsSuccessful = true
            });
        }

        return await Task.FromResult(new PaymentIn
        {
            HasCreditLimit = true,
            IsSuccessful = true
        });
    }
}