using CakeCompany.Application.Shipment;
using CakeCompany.Core.Interfaces;
using CakeCompany.Core.Interfaces.Transport;
using CakeCompany.Providers.Provider;
using CakeCompany.Providers.Provider.Delivery;
using CakeCompany.Providers.Provider.Delivery.Transport;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Enrichers;
using System.Text.Json;


//Inject all service dependency
var serviceProvider = new ServiceCollection()
          .AddScoped<IMediator, Mediator>()
          .AddScoped<IVan, Van>()
          .AddScoped<IShip, Ship>()
          .AddScoped<ITruck, Truck>()
          .AddScoped<ICakeProvider, CakeProvider>()
          .AddScoped<IDeliveryProvider, DeliveryProvider>()
          .AddScoped<IOrderProvider, OrderProvider>()
          .AddScoped<IPaymentProvider, PaymentProvider>()
          .AddScoped<ITransportProvider, TransportProvider>()
          .AddScoped<IShipmentProvider, ShipmentProvider>()
          .AddScoped<IDateTimeProvider, DateTimeProvider>()
          .AddMediatR(typeof(ShipmentHandler).Assembly)
          .AddLogging(configure => configure.AddSerilog())
          .BuildServiceProvider();


var level = Serilog.Events.LogEventLevel.Information;
#if DEBUG
level = Serilog.Events.LogEventLevel.Debug;
#endif

//Logging Configuration
Log.Logger = new LoggerConfiguration()
#if DEBUG
            .MinimumLevel.Debug()
#endif
       .WriteTo.File(
        path: "c:\\Logs\\consoleapp.log",
        restrictedToMinimumLevel: level,
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7)
       .Enrich.With(new ThreadIdEnricher())
       .WriteTo.Console(restrictedToMinimumLevel: level)
       .Enrich.With(new ThreadIdEnricher())
       .CreateLogger();

//Configure Global exception logging
AppDomain currentDomain = AppDomain.CurrentDomain;
currentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);

static void MyHandler(object sender, UnhandledExceptionEventArgs args)
{
    Exception e = (Exception)args.ExceptionObject;
    Log.Error(e.Message);
    Log.Error(e.StackTrace!);
}

//Send notification for shipment
var mediator = serviceProvider.GetService<IMediator>();
ArgumentNullException.ThrowIfNull(mediator);
var result = await mediator.Send(new ShipmentRequest(), CancellationToken.None);
Log.Information($"Shipped Product {JsonSerializer.Serialize(result)}");


