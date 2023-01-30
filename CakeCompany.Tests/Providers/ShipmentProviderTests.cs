using CakeCompany.Providers.Handlers.Orders.Query;
using MediatR;

namespace CakeCompany.Tests.Providers;

public class ShipmentProviderTests
{
    private IShipmentProvider shipmentProvider;
    private readonly Mock<IMediator> _mediator;
    private readonly Mock<ILogger<ShipmentProvider>> _logger;
    public ShipmentProviderTests()
    {
        _mediator = new Mock<IMediator>();
        _logger = new Mock<ILogger<ShipmentProvider>>();


    }
    [Fact]
    public async Task GetShipmentAsync_ArgumentNullException_Test()
    {
        //Arrange
        //Disabled CS8620 and CS8600 as it is require to test argument null exception
#pragma warning disable CS8620, CS8600
        _mediator.Setup(x => x.Send(It.IsAny<OrderRequest>(),CancellationToken.None)).ReturnsAsync(await Task.FromResult(null as Order[]));
#pragma warning restore CS8620, CS8600

        shipmentProvider = new ShipmentProvider(_mediator.Object, _logger.Object);

        //Act & Assert
        await Assert.ThrowsAnyAsync<ArgumentNullException>(async () => await shipmentProvider.GetShipmentAsync());
    }

    [Fact]
    public async Task GetShipmentAsync_Exception_For_Delivery_Failure_Test()
    {
        //Arrange
        _mediator.Setup(x => x.Send(It.IsAny<OrderRequest>(), CancellationToken.None)).ReturnsAsync(await Task.FromResult(new Order[]
        {
            new OrderBuilder().WithCake(Cake.RedVelvet).WithClientName("Test1")
                .WithEstimatedDeliveryDate(DateTime.Now).WithId(1).WithQuantity(120).Build()
        }));
        _mediator.Setup(x => x.Send(It.IsAny<BakeCakeRequest>(), CancellationToken.None)).ReturnsAsync(await Task.FromResult(new ProductBuilder()
               .WithId(Guid.NewGuid()).WithOrderId(1).WithQuantity(120)
               .WithCake(Cake.RedVelvet).Build()));
        _mediator.Setup(x => x.Send(It.IsAny<CheckCakeRequest>(), CancellationToken.None)).ReturnsAsync(await Task.FromResult(DateTime.Now.AddMinutes(-30)));
        //_mediator.Setup(x => x.Send(new DeliveryRequest(), CancellationToken.None)).Returns(Task.FromResult((Order[])null));
        //_mediator.Setup(x => x.Send(new OrderCommandRequest(), CancellationToken.None)).Returns(Task.FromResult((Order[])null));
        _mediator.Setup(x => x.Send(It.IsAny<PaymentRequest>(), CancellationToken.None)).ReturnsAsync(await Task.FromResult(new PaymentInBuilder().WithIsSuccessful(true).Build()));
        _mediator.Setup(x => x.Send(It.IsAny<TransportRequest>(), CancellationToken.None)).ReturnsAsync(await Task.FromResult(typeof(Van)));
       
        shipmentProvider = new ShipmentProvider(_mediator.Object, _logger.Object);

        //Act & Assert
        var ex = await Assert.ThrowsAnyAsync<Exception>(async () => await shipmentProvider.GetShipmentAsync());
        ex.Message.Should().Be("Product Delivery has been failed");

    }

    [Fact]
    public async Task GetShipmentAsync_NoOrderDetails_Test()
    {
        //Arrange
        shipmentProvider = new ShipmentProvider(_mediator.Object, _logger.Object);

        //Act & Assert
        var result = await shipmentProvider.GetShipmentAsync();
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetShipmentAsync_HappyPath_Test()
    {
        //Arrange   
        _mediator.Setup(x => x.Send(It.IsAny<OrderRequest>(), CancellationToken.None)).ReturnsAsync(await Task.FromResult(new Order[]
        {
             new OrderBuilder().WithCake(Cake.RedVelvet).WithClientName("Test1")
                .WithEstimatedDeliveryDate(DateTime.Now).WithId(1).WithQuantity(120).Build(),
            new OrderBuilder().WithCake(Cake.Chocolate).WithClientName("Test2")
                .WithEstimatedDeliveryDate(DateTime.Now).WithId(2).WithQuantity(121).Build(),
            new OrderBuilder().WithCake(Cake.Vanilla).WithClientName("Test3")
                .WithEstimatedDeliveryDate(DateTime.Now).WithId(3).WithQuantity(122).Build(),
            new OrderBuilder().WithCake(0).WithClientName("Test4")
                .WithEstimatedDeliveryDate(DateTime.Now).WithId(4).WithQuantity(123).Build()
        }));
        _mediator.SetupSequence(x => x.Send(It.IsAny<BakeCakeRequest>(), CancellationToken.None))
            .ReturnsAsync(await Task.FromResult(new ProductBuilder()
                .WithId(Guid.NewGuid()).WithOrderId(1).WithQuantity(120)
                .WithCake(Cake.RedVelvet).Build()))
            .ReturnsAsync(await Task.FromResult(new ProductBuilder()
                .WithId(Guid.NewGuid()).WithOrderId(2).WithQuantity(121)
                .WithCake(Cake.Chocolate).Build()))
            .ReturnsAsync(await Task.FromResult(new ProductBuilder()
                .WithId(Guid.NewGuid()).WithOrderId(3).WithQuantity(122)
                .WithCake(Cake.Vanilla).Build()));
        _mediator.SetupSequence(x => x.Send(It.IsAny< CheckCakeRequest>(), CancellationToken.None))
            .ReturnsAsync(await Task.FromResult(DateTime.Now.AddMinutes(-30)))
            .ReturnsAsync(await Task.FromResult(DateTime.Now.AddMinutes(-30)))
            .ReturnsAsync(await Task.FromResult(DateTime.Now.AddHours(-1)))
            .ReturnsAsync(await Task.FromResult(DateTime.Now.AddMinutes(-20)));
        _mediator.Setup(x => x.Send(It.IsAny< DeliveryRequest>(), CancellationToken.None)).ReturnsAsync(await Task.FromResult(true));
        //_mediator.Setup(x => x.Send(new OrderCommandRequest(), CancellationToken.None)).Returns(Task.FromResult((Order[])null));
        _mediator.SetupSequence(x => x.Send(It.IsAny< PaymentRequest>(), CancellationToken.None))
            .ReturnsAsync(await Task.FromResult(new PaymentInBuilder().WithIsSuccessful(true).Build()))
            .ReturnsAsync(await Task.FromResult(new PaymentInBuilder().WithIsSuccessful(true).Build()))
            .ReturnsAsync(await Task.FromResult(new PaymentInBuilder().WithIsSuccessful(true).Build()))
            .ReturnsAsync(await Task.FromResult(new PaymentInBuilder().WithIsSuccessful(false).Build()));
        
        _mediator.Setup(x => x.Send(It.IsAny<TransportRequest>(), CancellationToken.None)).ReturnsAsync(await Task.FromResult(typeof(Van)));


        shipmentProvider = new ShipmentProvider(_mediator.Object, _logger.Object);

        //Act
        var result = await shipmentProvider.GetShipmentAsync();

        //Assert or Verify
        result.Should().NotBeNull();
        result!.Count.Should().Be(3);

        _mediator.VerifyAll();
    }
}
