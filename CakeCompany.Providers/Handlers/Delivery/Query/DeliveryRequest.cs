using CakeCompany.Core.Dtos;
using CakeCompany.Core.Enums;
using MediatR;

public record DeliveryRequest : IRequest<bool>
{
    public List<Product> productList { get; set; }
    public Type transportType { get; set; } 
}