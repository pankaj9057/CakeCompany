using CakeCompany.Core.Dtos;
using CakeCompany.Core.Enums;
using MediatR;

public record BakeCakeRequest : IRequest<Product>
{
   public Order order { get; set; }
}