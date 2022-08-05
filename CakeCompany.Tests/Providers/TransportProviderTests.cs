namespace CakeCompany.Tests.Providers;

public class TransportProviderTests
{
    [Theory]
    [MemberData(nameof(TestData))]
    public async Task TransportProvider_HappyPath_Tests(double quantity, string expected)
    {
        //Arrange
        var transportProvider = new TransportProvider();

        //Act
        var result = await transportProvider.CheckForAvailability(new List<Product>
        {
            new ProductBuilder()
                .WithId(Guid.NewGuid()).WithOrderId(1).WithQuantity(quantity)
                .WithCake(Cake.RedVelvet).Build(),
            new ProductBuilder()
                .WithId(Guid.NewGuid()).WithOrderId(1).WithQuantity(quantity)
                .WithCake(Cake.Chocolate).Build(),
            new ProductBuilder()
                .WithId(Guid.NewGuid()).WithOrderId(1).WithQuantity(quantity)
                .WithCake(Cake.Vanilla).Build()
        });

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(expected);
    }


    //Test data
    public static IEnumerable<object[]> TestData()
    {
        yield return new object[] { 100, nameof(Van) };
        yield return new object[] { 1000, nameof(Truck) };
        yield return new object[] { 10000, nameof(Ship) };
    }
}
