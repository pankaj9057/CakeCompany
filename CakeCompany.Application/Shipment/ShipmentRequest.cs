using CakeCompany.Core.Dtos;
using MediatR;

namespace CakeCompany.Application.Shipment;

public record ShipmentRequest : IRequest<List<Product>?>;
