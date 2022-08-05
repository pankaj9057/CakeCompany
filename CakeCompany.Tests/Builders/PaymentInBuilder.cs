namespace CakeCompany.Tests.Builders;

internal class PaymentInBuilder
{
    private bool _isSuccess;
    public PaymentInBuilder WithIsSuccessful(bool isSuccess)
    {
        _isSuccess = isSuccess;
        return this;
    }

    public PaymentIn Build()
    {
        return new PaymentIn
        {
            HasCreditLimit = true,
            IsSuccessful = _isSuccess
        };
    }
}
