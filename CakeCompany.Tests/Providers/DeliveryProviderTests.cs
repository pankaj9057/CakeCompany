using CakeCompany.Core.Interfaces.Transport;
using CakeCompany.Providers.Provider.Delivery;

namespace CakeCompany.Tests.Providers;

public class DeliveryProviderTests
{
    private IDeliveryProvider _deliveryProvider;
    private readonly Mock<IDelivery<Van>> _van;
    private readonly Mock<IDelivery<Ship>> _ship;
    private readonly Mock<IDelivery<Truck>> _truck;

    public DeliveryProviderTests()
    {
        _van = new Mock<IDelivery<Van>>();
        _ship = new Mock<IDelivery<Ship>>();
        _truck = new Mock<IDelivery<Truck>>();
    }

    [Theory]
    [MemberData(nameof(TestData))]
    public async Task DeliveryProvider_HappyPath_Tests(bool expectedResult, Type transportType, int vanVerifyCount, int shipVerifyCount, int truckVerifyCount)
    {
        //Arrange
        _van.Setup(x => x.Deliver(It.IsAny<List<Product>>())).ReturnsAsync(await Task.FromResult(expectedResult)).Verifiable();
        _truck.Setup(x => x.Deliver(It.IsAny<List<Product>>())).ReturnsAsync(await Task.FromResult(expectedResult)).Verifiable();
        _ship.Setup(x => x.Deliver(It.IsAny<List<Product>>())).ReturnsAsync(await Task.FromResult(expectedResult)).Verifiable();
        _deliveryProvider = new DeliveryProvider(_van.Object, _truck.Object, _ship.Object);

        //Act
        var result = await _deliveryProvider.Deliver(
            new List<Product>
            {
                new ProductBuilder().WithQuantity(100).WithCake(Cake.Vanilla).WithId(Guid.NewGuid()).WithOrderId(1).Build(),
                new ProductBuilder().WithQuantity(100).WithCake(Cake.RedVelvet).WithId(Guid.NewGuid()).WithOrderId(1).Build()
            }
            , transportType);


        //Assert and Verify
        result.Should().Be(expectedResult);
        _van.Verify(x => x.Deliver(It.IsAny<List<Product>>()), Times.Exactly(vanVerifyCount));
        _truck.Verify(x => x.Deliver(It.IsAny<List<Product>>()), Times.Exactly(truckVerifyCount));
        _ship.Verify(x => x.Deliver(It.IsAny<List<Product>>()), Times.Exactly(shipVerifyCount));
    }


    //Test data
    public static IEnumerable<object[]> TestData()
    {
        yield return new object[] { true, typeof(Van), 1, 0, 0 };
        yield return new object[] { true, typeof(Truck), 0, 0, 1 };
        yield return new object[] { true, typeof(Ship), 0, 1, 0 };

        yield return new object[] { false, typeof(Van), 1, 0, 0 };
        yield return new object[] { false, typeof(Truck), 0, 0, 1 };
        yield return new object[] { false, typeof(Ship), 0, 1, 0 };

        yield return new object[] { false, typeof(string), 0, 0, 0 };
    }
}
