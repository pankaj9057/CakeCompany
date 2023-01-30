using CakeCompany.Core.Dtos;
using CakeCompany.Core.Enums;
using MediatR;

public record CheckCakeRequest : IRequest<DateTime>
{
   public Order order { get; set; }
}