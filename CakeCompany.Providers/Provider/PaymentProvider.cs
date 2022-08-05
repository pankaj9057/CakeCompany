using CakeCompany.Core.Dtos;
using CakeCompany.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace CakeCompany.Providers.Provider;

public class PaymentProvider : IPaymentProvider
{
    private readonly ILogger<PaymentProvider> _logger;
    public PaymentProvider(ILogger<PaymentProvider> logger)
    {
        _logger = logger;
    }
    public async Task<PaymentIn> Process(Order order)
    {
        if (order.ClientName.Contains("Important"))
        {
            _logger.LogInformation($"No credit is available for client {order.ClientName}");

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