namespace CakeCompany.Tests.Providers;

public class ShipmentProviderTests
{
    private IShipmentProvider shipmentProvider;
    private readonly Mock<IOrderProvider> _orderProvider;
    private readonly Mock<ICakeProvider> _cakeProvider;
    private readonly Mock<IPaymentProvider> _paymentProvider;
    private readonly Mock<ITransportProvider> _transportProvider;
    private readonly Mock<IDeliveryProvider> _deliveryProvider;
    private readonly Mock<ILogger<ShipmentProvider>> _logger;
    public ShipmentProviderTests()
    {
        _orderProvider = new Mock<IOrderProvider>();
        _cakeProvider = new Mock<ICakeProvider>();
        _paymentProvider = new Mock<IPaymentProvider>();
        _transportProvider = new Mock<ITransportProvider>();
        _deliveryProvider = new Mock<IDeliveryProvider>();
        _logger = new Mock<ILogger<ShipmentProvider>>();


    }
    [Fact]
    public async Task GetShipmentAsync_ArgumentNullException_Test()
    {
        //Arrange
        //Disabled CS8620 and CS8600 as it is require to test argument null exception
#pragma warning disable CS8620, CS8600
        _orderProvider.Setup(x => x.GetLatestOrders()).ReturnsAsync(await Task.FromResult((Order[])null));
#pragma warning restore CS8620, CS8600

        shipmentProvider = new ShipmentProvider(_orderProvider.Object, _cakeProvider.Object, _paymentProvider.Object
            , _transportProvider.Object, _deliveryProvider.Object, _logger.Object);

        //Act & Assert
        await Assert.ThrowsAnyAsync<ArgumentNullException>(async () => await shipmentProvider.GetShipmentAsync());
    }

    [Fact]
    public async Task GetShipmentAsync_Exception_For_Delivery_Failure_Test()
    {
        //Arrange
        _orderProvider.Setup(x => x.GetLatestOrders()).ReturnsAsync(await Task.FromResult(new Order[]
        {
            new OrderBuilder().WithCake(Cake.RedVelvet).WithClientName("Test1")
                .WithEstimatedDeliveryDate(DateTime.Now).WithId(1).WithQuantity(120).Build()
        }));

        _cakeProvider.Setup(x => x.Check(It.IsAny<Order>()))
           .ReturnsAsync(await Task.FromResult(DateTime.Now.AddMinutes(-30)));

        _cakeProvider.Setup(x => x.Bake(It.IsAny<Order>()))
           .ReturnsAsync(await Task.FromResult(new ProductBuilder()
               .WithId(Guid.NewGuid()).WithOrderId(1).WithQuantity(120)
               .WithCake(Cake.RedVelvet).Build()));

        _transportProvider.Setup(x => x.CheckForAvailability(It.IsAny<List<Product>>()))
            .ReturnsAsync(await Task.FromResult(nameof(Van)));

        _paymentProvider.Setup(x => x.Process(It.IsAny<Order>()))
            .ReturnsAsync(await Task.FromResult(new PaymentInBuilder().WithIsSuccessful(true).Build()));

        shipmentProvider = new ShipmentProvider(_orderProvider.Object, _cakeProvider.Object, _paymentProvider.Object
            , _transportProvider.Object, _deliveryProvider.Object, _logger.Object);

        //Act & Assert
        var ex = await Assert.ThrowsAnyAsync<Exception>(async () => await shipmentProvider.GetShipmentAsync());
        ex.Message.Should().Be("Product Delivery has been failed");

    }

    [Fact]
    public async Task GetShipmentAsync_NoOrderDetails_Test()
    {
        //Arrange
        shipmentProvider = new ShipmentProvider(_orderProvider.Object, _cakeProvider.Object, _paymentProvider.Object
            , _transportProvider.Object, _deliveryProvider.Object, _logger.Object);

        //Act & Assert
        var result = await shipmentProvider.GetShipmentAsync();
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetShipmentAsync_HappyPath_Test()
    {
        //Arrange
        _orderProvider.Setup(x => x.GetLatestOrders()).ReturnsAsync(await Task.FromResult(new Order[]
        {
            new OrderBuilder().WithCake(Cake.RedVelvet).WithClientName("Test1")
                .WithEstimatedDeliveryDate(DateTime.Now).WithId(1).WithQuantity(120).Build(),
            new OrderBuilder().WithCake(Cake.Chocolate).WithClientName("Test2")
                .WithEstimatedDeliveryDate(DateTime.Now).WithId(2).WithQuantity(121).Build(),
            new OrderBuilder().WithCake(Cake.Vanilla).WithClientName("Test3")
                .WithEstimatedDeliveryDate(DateTime.Now).WithId(3).WithQuantity(122).Build(),
            new OrderBuilder().WithCake(0).WithClientName("Test4")
                .WithEstimatedDeliveryDate(DateTime.Now).WithId(4).WithQuantity(123).Build(),
        })).Verifiable();

        _cakeProvider.SetupSequence(x => x.Check(It.IsAny<Order>()))
            .ReturnsAsync(await Task.FromResult(DateTime.Now.AddMinutes(-30)))
            .ReturnsAsync(await Task.FromResult(DateTime.Now.AddMinutes(-30)))
            .ReturnsAsync(await Task.FromResult(DateTime.Now.AddHours(-1)))
            .ReturnsAsync(await Task.FromResult(DateTime.Now.AddMinutes(-20)));

        _cakeProvider.SetupSequence(x => x.Bake(It.IsAny<Order>()))
            .ReturnsAsync(await Task.FromResult(new ProductBuilder()
                .WithId(Guid.NewGuid()).WithOrderId(1).WithQuantity(120)
                .WithCake(Cake.RedVelvet).Build()))
            .ReturnsAsync(await Task.FromResult(new ProductBuilder()
                .WithId(Guid.NewGuid()).WithOrderId(2).WithQuantity(121)
                .WithCake(Cake.Chocolate).Build()))
            .ReturnsAsync(await Task.FromResult(new ProductBuilder()
                .WithId(Guid.NewGuid()).WithOrderId(3).WithQuantity(122)
                .WithCake(Cake.Vanilla).Build()));

        _transportProvider.Setup(x => x.CheckForAvailability(It.IsAny<List<Product>>()))
            .ReturnsAsync(await Task.FromResult(nameof(Van)));

        _paymentProvider.SetupSequence(x => x.Process(It.IsAny<Order>()))
            .ReturnsAsync(await Task.FromResult(new PaymentInBuilder().WithIsSuccessful(true).Build()))
            .ReturnsAsync(await Task.FromResult(new PaymentInBuilder().WithIsSuccessful(true).Build()))
            .ReturnsAsync(await Task.FromResult(new PaymentInBuilder().WithIsSuccessful(true).Build()))
            .ReturnsAsync(await Task.FromResult(new PaymentInBuilder().WithIsSuccessful(false).Build()));

        _deliveryProvider.Setup(x => x.Deliver(It.IsAny<List<Product>>(), It.IsAny<string>()))
            .ReturnsAsync(await Task.FromResult(true));

        shipmentProvider = new ShipmentProvider(_orderProvider.Object, _cakeProvider.Object, _paymentProvider.Object
            , _transportProvider.Object, _deliveryProvider.Object, _logger.Object);

        //Act
        var result = await shipmentProvider.GetShipmentAsync();

        //Assert or Verify
        result.Should().NotBeNull();
        result!.Count.Should().Be(3);

        _orderProvider.Verify(x => x.GetLatestOrders(), Times.Once);
        _transportProvider.Verify(x => x.CheckForAvailability(It.IsAny<List<Product>>()), Times.Once);
        _cakeProvider.Verify(x => x.Bake(It.IsAny<Order>()), Times.AtLeast(3));
        _cakeProvider.Verify(x => x.Check(It.IsAny<Order>()), Times.AtLeast(4));
        _paymentProvider.Verify(x => x.Process(It.IsAny<Order>()), Times.AtLeast(4));
    }
}
