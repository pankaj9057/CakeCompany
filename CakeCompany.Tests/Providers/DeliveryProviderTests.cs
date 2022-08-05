using CakeCompany.Core.Interfaces.Transport;
using CakeCompany.Providers.Provider.Delivery;

namespace CakeCompany.Tests.Providers;

public class DeliveryProviderTests
{
    private IDeliveryProvider _deliveryProvider;
    private readonly Mock<IVan> _van;
    private readonly Mock<IShip> _ship;
    private readonly Mock<ITruck> _truck;

    public DeliveryProviderTests()
    {
        _van = new Mock<IVan>();
        _ship = new Mock<IShip>();
        _truck = new Mock<ITruck>();
    }

    [Theory]
    [MemberData(nameof(TestData))]
    public async Task DeliveryProvider_HappyPath_Tests(bool expectedResult, string transportType, int vanVerifyCount, int shipVerifyCount, int truckVerifyCount)
    {
        //Arrange
        _van.Setup(x => x.Deliver(It.IsAny<List<Product>>())).ReturnsAsync(await Task.FromResult(expectedResult)).Verifiable();
        _truck.Setup(x => x.Deliver(It.IsAny<List<Product>>())).ReturnsAsync(await Task.FromResult(expectedResult)).Verifiable();
        _ship.Setup(x => x.Deliver(It.IsAny<List<Product>>())).ReturnsAsync(await Task.FromResult(expectedResult)).Verifiable();
        _deliveryProvider = new DeliveryProvider(_van.Object, _ship.Object, _truck.Object);

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
        yield return new object[] { true, nameof(Van), 1, 0, 0 };
        yield return new object[] { true, nameof(Truck), 0, 0, 1 };
        yield return new object[] { true, nameof(Ship), 0, 1, 0 };

        yield return new object[] { false, nameof(Van), 1, 0, 0 };
        yield return new object[] { false, nameof(Truck), 0, 0, 1 };
        yield return new object[] { false, nameof(Ship), 0, 1, 0 };

        yield return new object[] { false, "Unknown", 0, 0, 0 };
    }
}
