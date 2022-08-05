namespace CakeCompany.Tests.Providers;

public class CakeProviderTests
{

    private ICakeProvider _provider;
    private readonly Mock<IDateTimeProvider> _dateTimeProvider;

    public CakeProviderTests()
    {
        _dateTimeProvider = new Mock<IDateTimeProvider>();
    }

    [Theory]
    [MemberData(nameof(TestData))]
    public async Task CakeProvider_HappyPath_Tests(DateTime currentDate, Cake cake, DateTime expectedDate)
    {
        //Arrange
        _provider = new CakeProvider(_dateTimeProvider.Object);
        _dateTimeProvider.Setup(x => x.Now()).Returns(currentDate);

        //Act
        var result = await _provider.Check(
            new OrderBuilder()
            .WithClientName("Test")
            .WithId(1)
            .WithQuantity(100)
            .WithCake(cake)
            .WithEstimatedDeliveryDate(DateTime.Now)
            .Build());

        //Assert
        result.Should().Be(expectedDate);

    }

    //Test data
    public static IEnumerable<object[]> TestData()
    {
        var currentDate = DateTime.Now;
        yield return new object[] { currentDate, Cake.Vanilla, currentDate.Add(TimeSpan.FromHours(15)) };
        yield return new object[] { currentDate, Cake.RedVelvet, currentDate.Add(TimeSpan.FromMinutes(60)) };
        yield return new object[] { currentDate, Cake.Chocolate, currentDate.Add(TimeSpan.FromMinutes(30)) };
    }
}
