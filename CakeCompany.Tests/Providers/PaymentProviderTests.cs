namespace CakeCompany.Tests.Providers;

public class PaymentProviderTests
{
    private readonly Mock<ILogger<PaymentProvider>> _logger;
    public PaymentProviderTests()
    {
        _logger = new Mock<ILogger<PaymentProvider>>();
    }
    [Fact]
    public async Task PaymentProvider_HappyPath_Tests()
    {
        //Arrange
        var paymentProvider = new PaymentProvider(_logger.Object);

        //Act
        var result = await paymentProvider.Process(new OrderBuilder().WithCake(Cake.RedVelvet).WithClientName("Test1")
            .WithEstimatedDeliveryDate(DateTime.Now).WithId(1).WithQuantity(120).Build());

        //Assert
        result.Should().NotBeNull();
        result.HasCreditLimit.Should().BeTrue();
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public async Task PaymentProvider_HappyPath_ImportantClinet_Tests()
    {
        //Arrange
        var paymentProvider = new PaymentProvider(_logger.Object);

        //Act
        var result = await paymentProvider.Process(new OrderBuilder().WithCake(Cake.RedVelvet).WithClientName("Important")
            .WithEstimatedDeliveryDate(DateTime.Now).WithId(1).WithQuantity(120).Build());

        //Assert
        result.Should().NotBeNull();
        result.HasCreditLimit.Should().BeFalse();
        result.IsSuccessful.Should().BeTrue();
    }
}
